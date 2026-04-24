/**
 * Ralph loop. Intentionally boring.
 *
 * Each iteration:
 *   1. Check specs/ for any unchecked `- [ ]` item. If none, stop.
 *   2. Spawn a *fresh* pi session (no context carry-over).
 *   3. Ask it to run the `/ralph` skill.
 *   4. Run `dotnet test`. If green, commit. If red (or the agent errored),
 *      hard-reset the working tree so the next iteration starts clean.
 *
 * Tuning happens in .pi/skills/ralph/SKILL.md, not here.
 */

import { AuthStorage, ModelRegistry, createAgentSession, SessionManager } from "@mariozechner/pi-coding-agent";
import { execSync, spawnSync } from "node:child_process";
import { readdirSync, readFileSync, existsSync } from "node:fs";

// Optional model override via env vars:
//   RALPH_PROVIDER=anthropic RALPH_MODEL=claude-sonnet-4-5 npx tsx ralph.ts
//   RALPH_PROVIDER=ollama    RALPH_MODEL=gemma4:26b       npx tsx ralph.ts
// If unset, pi's default (from settings.json / first available) is used.
const PROVIDER = process.env.RALPH_PROVIDER;
const MODEL_ID = process.env.RALPH_MODEL;
const THINKING = (process.env.RALPH_THINKING ?? "off") as
    | "off" | "minimal" | "low" | "medium" | "high" | "xhigh";

// ModelRegistry resolves BOTH built-in models AND custom ones from
// ~/.pi/agent/models.json (Ollama, LM Studio, vLLM, etc).
const authStorage = AuthStorage.create();
const modelRegistry = ModelRegistry.create(authStorage);
const model = PROVIDER && MODEL_ID ? modelRegistry.find(PROVIDER, MODEL_ID) : undefined;
if (PROVIDER && MODEL_ID && !model) {
    console.error(`Model not found: ${PROVIDER}/${MODEL_ID}`);
    console.error(`Check it exists in ~/.pi/agent/models.json or is a built-in provider/id.`);
    process.exit(1);
}
if (model) console.log(`Using model: ${PROVIDER}/${MODEL_ID} (thinking=${THINKING})`);

const MAX_ITERATIONS = 200;
const SPECS_DIR = "specs";

function hasUncheckedWork(): boolean {
    if (!existsSync(SPECS_DIR)) return false;
    return readdirSync(SPECS_DIR)
        .filter((f) => f.endsWith(".md"))
        .some((f) => /^\s*-\s*\[\s\]/m.test(readFileSync(`${SPECS_DIR}/${f}`, "utf8")));
}

function sh(cmd: string, opts: { check?: boolean } = {}): number {
    const r = spawnSync(cmd, { shell: true, stdio: "inherit" });
    if (opts.check && r.status !== 0) throw new Error(`failed: ${cmd}`);
    return r.status ?? 1;
}

function testsPass(): boolean {
    // No test project yet = treat as "pass" so bootstrap can proceed.
    const hasCsproj = execSync("find . -name '*.csproj' -not -path '*/bin/*' -not -path '*/obj/*' 2>/dev/null || true")
        .toString()
        .trim();
    if (!hasCsproj) return true;
    return sh("dotnet test --nologo --verbosity quiet") === 0;
}

function gitHeadSha(): string {
    return execSync("git rev-parse --short HEAD").toString().trim();
}

async function runOnce(iter: number): Promise<"ok" | "no-change" | "failed"> {
    const before = gitHeadSha();

    const { session } = await createAgentSession({
        sessionManager: SessionManager.inMemory(), // fresh context every iteration
        authStorage,
        modelRegistry,
        ...(model ? { model, thinkingLevel: THINKING } : {}),
    });

    // Streaming output: assistant text + one-line-per-tool-call with args.
    session.subscribe((e) => {
        if (e.type === "message_update" && e.assistantMessageEvent.type === "text_delta") {
            process.stdout.write(e.assistantMessageEvent.delta);
        }
        if (e.type === "tool_execution_start") {
            process.stdout.write(`\n  · ${formatToolCall(e.toolName, e.args)}\n`);
        }
        if (e.type === "tool_execution_end" && e.isError) {
            process.stdout.write(`    ↳ ERROR\n`);
        }
    });

    try {
        await session.prompt("/ralph");
    } catch (err) {
        console.error(`\n[iter ${iter}] agent error:`, err);
        return "failed";
    } finally {
        session.dispose();
    }

    // Did anything actually change on disk?
    const dirty = execSync("git status --porcelain").toString().trim();
    if (!dirty) {
        console.log(`\n[iter ${iter}] agent made no changes`);
        return "no-change";
    }

    if (!testsPass()) {
        console.log(`\n[iter ${iter}] tests failed — rolling back`);
        sh("git reset --hard HEAD");
        sh("git clean -fd");
        return "failed";
    }

    sh(`git add -A && git commit -m "ralph: iter ${iter}" --no-verify`);
    const after = gitHeadSha();
    console.log(`\n[iter ${iter}] ✓ ${before} → ${after}`);
    return "ok";
}

function truncate(s: string, max = 120): string {
    const oneLine = String(s ?? "").replace(/\s+/g, " ").trim();
    return oneLine.length > max ? oneLine.slice(0, max - 1) + "…" : oneLine;
}

function formatToolCall(name: string, args: any): string {
    const a = args ?? {};
    switch (name) {
        case "bash":
            return `bash  $ ${truncate(a.command)}`;
        case "read": {
            const range = a.offset || a.limit ? ` [${a.offset ?? 0}..+${a.limit ?? "?"}]` : "";
            return `read  ${a.path}${range}`;
        }
        case "write":
            return `write ${a.path} (${(a.content ?? "").length} bytes)`;
        case "edit": {
            const n = Array.isArray(a.edits) ? a.edits.length : 0;
            return `edit  ${a.path} (${n} change${n === 1 ? "" : "s"})`;
        }
        case "grep":
            return `grep  ${truncate(a.pattern)}  in ${a.path ?? "."}`;
        case "find":
            return `find  ${a.path ?? "."}  ${a.pattern ? `name=${a.pattern}` : ""}`.trim();
        case "ls":
            return `ls    ${a.path ?? "."}`;
        default:
            return `${name}  ${truncate(JSON.stringify(a))}`;
    }
}

async function main() {
    // Sanity: must be in a git repo.
    try {
        execSync("git rev-parse --git-dir", { stdio: "ignore" });
    } catch {
        console.error("Not in a git repo. Run: git init && git add -A && git commit -m init");
        process.exit(1);
    }

    let consecutiveFailures = 0;
    for (let i = 0; i < MAX_ITERATIONS; i++) {
        if (!hasUncheckedWork()) {
            console.log("\n🎉 All specs complete.");
            return;
        }
        console.log(`\n========== iteration ${i} ==========`);
        const result = await runOnce(i);
        if (result === "ok") consecutiveFailures = 0;
        else consecutiveFailures++;

        if (consecutiveFailures >= 3) {
            console.error("\n3 failures in a row — bailing. Inspect specs and PROGRESS.md.");
            process.exit(2);
        }
    }
    console.log(`\nReached MAX_ITERATIONS=${MAX_ITERATIONS}.`);
}

main().catch((e) => {
    console.error(e);
    process.exit(1);
});
