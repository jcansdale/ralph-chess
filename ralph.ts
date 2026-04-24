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

import { createAgentSession, SessionManager } from "@mariozechner/pi-coding-agent";
import { execSync, spawnSync } from "node:child_process";
import { readdirSync, readFileSync, existsSync } from "node:fs";

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
    });

    // Minimal streaming output so you can see what it's doing.
    session.subscribe((e) => {
        if (e.type === "message_update" && e.assistantMessageEvent.type === "text_delta") {
            process.stdout.write(e.assistantMessageEvent.delta);
        }
        if (e.type === "tool_execution_start") {
            process.stdout.write(`\n  · ${e.toolName}\n`);
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
