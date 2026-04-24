# 060 — Make/Unmake and legality

Wire up move application and the legality filter. After this spec,
`MoveGenerator.GenerateLegal(Position)` returns only moves that do
not leave the mover in check.

## Items

- [ ] `Position.MakeMove(Move)` applies a move and returns an
  `UndoInfo` struct; `Position.UndoMove(Move, UndoInfo)` restores
  exact prior state (including castling rights, ep square, halfmove
  clock, captured piece). Round-trip test: for each move in each of
  50 random reachable positions, make+unmake yields byte-identical FEN.
- [ ] `bool Position.IsSquareAttacked(Square sq, Color byColor)` —
  used for check detection and castling legality. Tests: in the
  start position, all squares on rank 3 are attacked by White,
  none on ranks 5–8.
- [ ] `bool Position.IsInCheck(Color c)` helper built on
  `IsSquareAttacked`.
- [ ] `MoveGenerator.GenerateLegal(Position)` — generates pseudo-legal
  moves, applies each, rejects if the mover is in check after the
  move, unmakes, yields the survivors. Castling additionally requires
  that the king is not currently in check and does not pass through
  an attacked square.
- [ ] Tests:
  - Start position has 20 legal moves.
  - `"rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2"`
    (after 1. e4 e5) has 29 legal moves for White.
  - A position where the king is in check by one piece and can only
    block — the legal move list is limited to king moves + block +
    capture-checker.
