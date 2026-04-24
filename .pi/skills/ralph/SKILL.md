---
name: ralph
description: Ralph loop protocol — do exactly one spec item and stop.
---

# The Ralph protocol

You are being invoked inside a loop. A **fresh** copy of you runs for
every spec item. Assume you remember nothing; the files on disk and
`PROGRESS.md` are your only memory.

Do these steps, in order, once, then stop.

## 1. Orient

- Read `AGENTS.md` in full. Its rules are non-negotiable.
- Read `PROGRESS.md` to see what previous iterations have completed.
- List `specs/` alphabetically. Open each file until you find the first
  one containing an unchecked `- [ ]` item.

## 2. Pick exactly one item

- The item = the **first** unchecked `- [ ]` in that spec file, scanned
  top-to-bottom.
- Read the full spec file it lives in, plus any files it references.
- If the item is ambiguous or depends on work not yet done: do NOT
  guess. Insert a line `> QUESTION: <your question>` directly beneath
  the item, save, and stop. Do not tick the box.

## 3. Implement

- Make the smallest change that satisfies the item's acceptance
  criteria. No scope creep. Do not start the next item "while you're
  there".
- Add or update tests as the spec requires. Tests are the oracle —
  if the spec says "perft(4) from Kiwipete = 4085603", that number
  must appear in a test.
- Obey every rule in `AGENTS.md` (no TODOs, warnings-as-errors,
  nullable enabled, project layout, style).

### File layout: prefer partial classes, one feature per file

You have no conversation memory between iterations and the `edit`
tool can be brittle. To stay robust, prefer **writing new files** over
**editing existing ones**. Concretely:

- Any class that will gain methods across multiple iterations
  (`Position`, `MoveGenerator`, `Board`, etc.) **must** be declared
  `partial`. Mark it `partial` the first time you create it.
- When you add a new method or a cohesive group of methods, put them
  in their own file named `<ClassName>.<Feature>.cs`, e.g.
  `Position.FromFen.cs`, `Position.ToFen.cs`,
  `MoveGenerator.Pawns.cs`. Each such file declares
  `public partial class <ClassName> { ... }`.
- Do the same for test classes:
  `PositionTests.FromFen.cs`, `PositionTests.ToFen.cs`. Declare
  `public partial class <ClassName>Tests`.
- This means: adding a method = `write` a new file, not `edit` an
  existing one. Editing is still fine for tiny, surgical fixes
  (renaming a symbol, flipping a branch condition), but whole-method
  additions should always be new files.

Example skeleton for a new feature:

```csharp
// src/ChessEngine/Position.ToFen.cs
namespace ChessEngine;

public sealed partial class Position
{
    public string ToFen() { /* … */ }
}
```

## 4. Verify locally

Run:

```
dotnet build --nologo -v quiet
dotnet test  --nologo -v quiet
```

Both must be green. If red, fix before stopping. If you cannot make
them green, revert your changes (`git checkout -- .`) rather than
leaving a broken tree — the outer loop will hard-reset anyway, but
a clean exit is tidier.

## 5. Record

- Flip `- [ ]` to `- [x]` on the item you completed. Nothing else in
  the spec file changes.
- Append ONE line to `PROGRESS.md`:
  ```
  YYYY-MM-DD  <spec-file>  <≤80-char summary>
  ```

## 6. Stop

Do not start a second item. Do not "polish". Do not commit — the
outer loop commits for you. Just stop.

## Anti-patterns (will get your iteration reverted)

- Ticking a box without passing tests.
- Editing multiple spec files.
- Adding NuGet packages not permitted by `AGENTS.md`.
- Leaving `NotImplementedException` or `// TODO` behind.
- Restructuring directories.
- Modifying `ralph.ts`, `.pi/`, `AGENTS.md`, or `README.md`.
