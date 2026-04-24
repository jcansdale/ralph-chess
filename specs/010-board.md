# 010 — Board, pieces, FEN

Core value types and position representation. No move generation yet.

## Items

- [ ] `readonly struct Square` wrapping a `byte` 0..63. Static helpers
  `Square.FromFileRank(int file, int rank)`, `File`, `Rank`,
  `ToString()` returning algebraic (e.g. `"e4"`). Unit tests cover
  round-trip for all 64 squares.
- [ ] `enum Color { White = 0, Black = 1 }` and
  `enum PieceType { None, Pawn, Knight, Bishop, Rook, Queen, King }`.
  `readonly struct Piece(Color, PieceType)` with `None` static, plus
  `char` conversion (`P n b R q K` etc, uppercase = white). Tests
  cover every piece char round-trip.
- [ ] `sealed class Position` holds: piece array `Piece[64]`, side to
  move, castling rights (flags enum `CastlingRights`), en-passant
  target `Square?`, halfmove clock, fullmove number. Expose `Clone()`.
- [ ] `Position.FromFen(string)` and `Position.ToFen()`. Round-trip
  tests for: start position, Kiwipete
  (`r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1`),
  an en-passant position, a position with only kings. Must produce
  byte-identical FEN after parse+emit.
- [ ] Start-position FEN constant `Position.StartFen` and
  `Position.StartingPosition()` factory.
