# Ralph Loop Example: C# Chess Engine

A worked example of the **Ralph Wiggum Technique** using
[pi](https://www.npmjs.com/package/@mariozechner/pi-coding-agent):
a dumb outer loop that repeatedly spawns a fresh pi session, each of which
picks the next unchecked item from `specs/` and implements it.

Target: a minimal but correct chess engine in C# with a UCI interface,
built incrementally, one spec at a time, with tests gating progress.

## How it works

```
 ┌──────────────────────────────────────────────────┐
 │ ralph.ts  (the loop — ~60 lines, boring on purpose) │
 │                                                  │
 │   while any spec has `- [ ]`:                    │
 │     fresh pi session                             │
 │     session.prompt("/ralph")   ◀── skill does the thinking
 │     if green tests: git commit                   │
 │     else:          git reset --hard              │
 └──────────────────────────────────────────────────┘
                       │
                       ▼
 ┌──────────────────────────────────────────────────┐
 │ .pi/skills/ralph/SKILL.md                        │
 │   The *protocol*. Edit this to change behaviour. │
 │   The outer loop never changes.                  │
 └──────────────────────────────────────────────────┘
                       │
                       ▼
 ┌──────────────────────────────────────────────────┐
 │ specs/*.md        AGENTS.md        PROGRESS.md   │
 │ one spec per      invariants       append-only   │
 │ unit of work      (test cmd, etc.) agent journal │
 └──────────────────────────────────────────────────┘
```

## Run it

Prereqs: Node 20+, .NET 8 SDK, `git`, an API key configured for `pi`
(`pi` once interactively to set one up, or export `ANTHROPIC_API_KEY`).

Each run lives on its own branch (`run-1`, `run-2`, …). `main` holds
the base scaffold and dev edits only; iteration commits never land on
it. Create a new run branch before kicking off:

```bash
npm install
git checkout -b run-1
cp ralph.config.example.json ralph.config.json   # edit to pick your model
git add ralph.config.json
git commit -m "run-1: configure model"
npx tsx ralph.ts
```

To compare runs side by side, just use another branch:

```bash
git checkout main
git checkout -b run-2
# edit ralph.config.json to a different model
git add -A && git commit -m "run-2: sonnet"
npx tsx ralph.ts
```

Then `gh compare run-1...run-2` or just `git log run-1 run-2 --oneline --graph`.

### Model selection

Precedence (highest to lowest):

1. Env vars: `RALPH_PROVIDER`, `RALPH_MODEL`, `RALPH_THINKING`
2. `ralph.config.json` in the working directory
3. pi default (`~/.pi/agent/settings.json`)

Env-var override is handy for quick ad-hoc runs without touching committed config:

```bash
RALPH_PROVIDER=anthropic RALPH_MODEL=claude-sonnet-4-5 npx tsx ralph.ts
```

## Key design choices (and why)

- **Fresh session per iteration** — avoids context rot, which is the
  single biggest failure mode of long agent runs.
- **A pi skill, not a prompt string** — the "real" prompt lives in
  `.pi/skills/ralph/SKILL.md`. Edit one file to iterate on behaviour;
  never touch `ralph.ts`.
- **Sharded specs** — one concern per file. The agent never has to load
  the whole plan into context.
- **Tests as the oracle** — `perft` tests (move-generation counts at
  fixed depths) are the ground truth for chess correctness. The loop
  only commits when `dotnet test` is green.
- **Git as the undo buffer** — bad iterations are hard-reset. No manual
  cleanup.
- **PROGRESS.md as working memory** — the agent writes one line per
  completed item, readable by the next (fresh-context) iteration.

## Layout

```
ralph.ts                    # the loop
package.json
AGENTS.md                   # project-wide rules seen by every pi session
PROGRESS.md                 # append-only journal
.pi/skills/ralph/SKILL.md   # the protocol the agent follows on `/ralph`
specs/
  000-bootstrap.md          # create solution + test project
  010-board.md              # board + pieces + FEN
  020-moves-pawn.md         # pawn moves (incl. en passant, promotion)
  030-moves-knight.md
  040-moves-sliders.md      # bishop/rook/queen
  050-moves-king.md         # incl. castling
  060-legality.md           # filter moves that leave own king in check
  070-perft.md              # perft tests at standard positions (the oracle)
  080-eval.md               # material + piece-square tables
  090-search.md             # negamax + alpha-beta + iterative deepening
  100-uci.md                # UCI protocol loop
  110-cli-play.md           # simple human-vs-engine REPL
```

## What the agent will NOT do well unaided

- Big architectural rewrites mid-spec. Keep specs small.
- Performance tuning without a benchmark spec. Add one if you care.
- Anything not checked by a test. If it matters, write a `- [ ]` test item.
