# 070 — Perft: the move-generation oracle

`perft(depth)` counts the number of leaf nodes in a full legal-move
search tree to the given depth. It is the single most important
correctness check for a chess engine. Published perft values are
exact — any deviation means a bug.

## Items

- [ ] `public static long Perft(Position p, int depth)` in a new
  static class `ChessEngine.Perft`. Uses `GenerateLegal` + make/unmake.
- [ ] Tests (all numbers from chessprogramming.org; every one MUST
  match exactly):
  - Start position: perft(1)=20, perft(2)=400, perft(3)=8902, perft(4)=197281.
  - Kiwipete `r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1`:
    perft(1)=48, perft(2)=2039, perft(3)=97862, perft(4)=4085603.
  - Position 3 `8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1`:
    perft(1)=14, perft(2)=191, perft(3)=2812, perft(4)=43238, perft(5)=674624.
  - Position 4 `r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2pP/R2Q1RK1 w kq - 0 1`:
    perft(1)=6, perft(2)=264, perft(3)=9467.
- [ ] Mark any perft test taking >30s as `[Trait("slow","true")]` and
  exclude from the default test run via filter in the test csproj
  so the Ralph loop stays fast. Keep at minimum perft(3) for every
  position in the fast suite.
