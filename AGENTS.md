# AGENTS.md — invariants for every pi session in this repo

You are contributing to a **C# chess engine** being built incrementally
via the Ralph loop. Read this file in full before doing anything.

## Non-negotiables

1. **Language / runtime**: C# 12, .NET 8. No other languages. No external
   NuGet dependencies beyond `Microsoft.NET.Test.Sdk`, `xunit`,
   `xunit.runner.visualstudio`.
2. **Project layout** (create on bootstrap, never restructure after):
   ```
   src/ChessEngine/ChessEngine.csproj       # class library
   src/ChessEngine.Cli/ChessEngine.Cli.csproj  # executable (UCI + REPL)
   tests/ChessEngine.Tests/ChessEngine.Tests.csproj
   ChessEngine.sln
   ```
3. **Test command** (the loop runs this — it MUST pass before commit):
   ```
   dotnet test --nologo --verbosity quiet
   ```
4. **Build warnings are errors**. Every csproj has
   `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` and
   `<Nullable>enable</Nullable>`.
5. **No TODOs, no `throw new NotImplementedException()`** left in code
   after a spec item is ticked. If you can't finish, don't tick the box.
6. **Never edit `specs/` except to tick `- [ ]` → `- [x]`**. Do not
   rewrite acceptance criteria. If a spec is wrong, add a
   `> QUESTION:` line under the offending item and stop.
7. **Never edit `ralph.ts`, `.pi/`, `AGENTS.md`, or `README.md`**.

## Style

- 4-space indent, `PascalCase` types/methods, `_camelCase` private fields.
- Prefer `readonly struct` for hot value types (`Move`, `Square`).
- Public API in `ChessEngine` namespace; search internals in
  `ChessEngine.Search`.
- XML doc comments on all public members.

## Chess conventions

- Squares: 0 = a1, 7 = h1, 56 = a8, 63 = h8 (little-endian rank-file).
- Colors: `White = 0`, `Black = 1`.
- Piece enum order: `None, Pawn, Knight, Bishop, Rook, Queen, King`.
- FEN is the canonical string form; support parse + emit from day one.
- Perft counts at standard positions are the ground truth for move
  generation. Use Kiwipete, position 3, position 4, etc. from the
  chessprogramming.org perft page.

## Progress journal

After completing a spec item, append ONE line to `PROGRESS.md`:

```
YYYY-MM-DD  spec-file  short-summary
```

That's how future (fresh-context) iterations learn what's already done
without re-reading all the code.
