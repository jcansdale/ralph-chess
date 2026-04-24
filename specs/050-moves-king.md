# 050 — King moves and castling

## Items

- [ ] King one-step moves to up to 8 surrounding squares; skip own
  pieces; include captures. Precomputed `Attacks.King(int sq)` table.
- [ ] Castling move generation (pseudo-legal, legality filter comes
  in spec 060). Emit O-O / O-O-O when:
  - the relevant castling right is set in `Position.CastlingRights`,
  - squares between king and rook are empty.
  Legality (king not in / through / into check) is NOT checked here.
- [ ] Tests:
  - Lone king in the centre has 8 moves.
  - From Kiwipete start (`r3k2r/.../R3K2R w KQkq - 0 1` with empty
    castle paths), White's pseudo-legal move list contains both
    `e1g1` and `e1c1`.
  - With `CastlingRights.None`, neither castle appears.
  - With a piece between king and rook, the corresponding castle
    does not appear.
