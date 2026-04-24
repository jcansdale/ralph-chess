# 010 — Board, pieces, FEN

Core value types and position representation. No move generation yet.

## Items

- [x] `readonly struct Square` wrapping a `byte` 0..63. Static helpers
  `Square.FromFileRank(int file, int rank)`, `File`, `Rank`,
  `ToString()` returning algebraic (e.g. `"e4"`). Unit tests cover
  round-trip for all 64 squares.
- [x] `enum Color { White = 0, Black = 1 }` and
  `enum PieceType { None, Pawn, Knight, Bishop, Rook, Queen, King }`.
  `readonly struct Piece(Color, PieceType)` with `None` static, plus
  `char` conversion (`P n b R q K` etc, uppercase = white). Tests
  cover every piece char round-trip.
- [ ] `sealed class Position` holds: piece array `Piece[64]`, side to
  move, castling rights (flags enum `CastlingRights`), en-passant
  target `Square?`, halfmove clock, fullmove number. Expose `Clone()`.
- [ ] `Position.ToFen()` — emit a FEN string from a Position. Unit
  test: construct the starting position *manually* (put each piece on
  its correct square by index: `Board[0]=WR, Board[1]=WN, ..., Board[63]=BR`)
  and assert `ToFen()` equals the start FEN string exactly.
  > HINT: FEN lists ranks **top-first** (rank 8 first, rank 1 last).
  > Our square convention: `a1=0`, `h1=7`, `a8=56`, `h8=63`, so
  > `index = rank*8 + file`. When emitting, walk `rank = 7` down to `0`.
- [ ] `Position.FromFen(string)` — parse a FEN string into a Position.
  Round-trip tests (parse then emit must be byte-identical to the input):
  start position, Kiwipete
  (`r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1`),
  a valid en-passant position (e.g. `rnbqkbnr/ppp1pppp/8/3pP3/8/8/PPPP1PPP/RNBQKBNR w KQkq d6 0 3`),
  a position with only kings (e.g. `4k3/8/8/8/8/8/8/4K3 w - - 0 1`).
  > HINT: the FEN placement has ranks in top-first order. The FEN
  > rank at string position `r` (0 = first in string = rank 8)
  > corresponds to board rank `7 - r`. So the *first* rank-string
  > you parse writes to board squares `56..63`, the last to `0..7`.
  > NOT monotonically incrementing from 0.
- [ ] Start-position FEN constant `Position.StartFen` and
  `Position.StartingPosition()` factory.
