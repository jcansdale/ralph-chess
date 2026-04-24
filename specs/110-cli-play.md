# 110 — Human-vs-engine REPL

A tiny interactive mode so a human can play the engine from a
terminal without a UCI GUI.

## Items

- [ ] `ChessEngine.Cli/Program.cs`: when invoked with `play` as the
  first arg, launch a REPL:
  - print the board (ASCII, 8 ranks × 8 files, white at the bottom)
  - prompt `your move (uci or 'quit'): `
  - accept moves in UCI (`e2e4`, `e7e8q`), reject illegal input with
    a clear message and re-prompt,
  - after the human's move, run the searcher with `movetime=1000`,
    apply the engine's reply, print the board, loop.
  - detect checkmate / stalemate / 50-move rule and announce the
    result, then exit.
- [ ] `Position.ToAsciiBoard()` helper used by the REPL and by
  debugging. Test: the start position renders to a fixed expected
  string (snapshot).
- [ ] Integration test driven through `StringReader`/`StringWriter`:
  feed `e2e4\nquit\n`, assert the transcript contains an engine
  reply in UCI form and that the session exits cleanly.
