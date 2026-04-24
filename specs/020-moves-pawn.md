# 020 — Move type and pawn move generation

Introduce the `Move` type and generate **pseudo-legal** pawn moves
(legality filtering happens in spec 060).

## Items

- [x] `readonly struct Move` packing: from `Square`, to `Square`,
  promotion `PieceType` (None if not a promo), flags for
  double-push / en-passant / capture. Provide `ToUci()` returning
  UCI notation (e.g. `"e2e4"`, `"e7e8q"`). Tests cover UCI emission
  for quiet, capture, double-push, ep, and all four promo types.
- [ ] Static class `MoveGenerator` with
  `IEnumerable<Move> GeneratePseudoLegal(Position p)`. For this spec
  implement **only pawn moves**: single push, double push from
  starting rank, diagonal captures, en-passant captures, and
  promotions (to N, B, R, Q) on the last rank. Non-pawn pieces
  generate nothing yet.
- [ ] Tests: from the start position, exactly 16 pawn moves are
  generated for White and exactly 16 for Black (after flipping
  side to move). From a crafted FEN with an en-passant target,
  the ep capture appears in the list exactly once. From a position
  with a pawn on the 7th rank, exactly 4 promotion moves are
  generated for that pawn (to N, B, R, Q).
