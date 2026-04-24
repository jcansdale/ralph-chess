# 080 — Evaluation

Static evaluation in centipawns from the side-to-move's perspective
(positive = good for side to move).

## Items

- [ ] Material values: P=100, N=320, B=330, R=500, Q=900, K=0 (kings
  aren't captured). `ChessEngine.Evaluation.Material(Position)` returns
  white_material − black_material in centipawns.
- [ ] Piece-square tables (PSTs): standard midgame tables for each
  piece type (white perspective; mirror ranks for black). Use the
  "simplified evaluation function" tables from chessprogramming.org.
  Add to material to produce `Evaluation.Evaluate(Position)` returning
  a score from side-to-move's perspective
  (i.e. negate if `SideToMove == Black`).
- [ ] Tests:
  - Start position evaluates to 0.
  - After `1.e4` (white to move flipped, now black to move), evaluation
    from Black's perspective is negative (White has centre-pawn bonus).
  - A position White-up-a-queen evaluates to roughly +900 cp from
    White's perspective (tolerate ±100 cp for PST noise).
