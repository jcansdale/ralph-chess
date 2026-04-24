# 030 — Knight moves

## Items

- [ ] Extend `MoveGenerator.GeneratePseudoLegal` to emit knight moves.
  Knights jump to up to 8 offsets; skip squares occupied by own pieces;
  captures of opposing pieces are included.
- [ ] Precompute a `ulong[64]` knight-attack bitboard table at static
  init, used for move generation. Expose it as
  `internal static class Attacks { public static ulong Knight(int sq); }`.
- [ ] Tests: from start position there are exactly 4 knight moves.
  From `"4k3/8/8/3N4/8/8/8/4K3 w - - 0 1"` the knight on d5 has
  exactly 8 target squares. A knight surrounded by own pieces
  generates 0 moves.
