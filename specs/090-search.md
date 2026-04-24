# 090 — Search: negamax + alpha-beta + iterative deepening

## Items

- [ ] `ChessEngine.Search.Searcher` with entry point
  `SearchResult Find(Position p, SearchLimits limits)` where
  `SearchLimits` has `int? MaxDepth` and `TimeSpan? MaxTime` and
  `SearchResult` has `Move BestMove`, `int ScoreCp`, `int Depth`,
  `long Nodes`, `TimeSpan Elapsed`, `IReadOnlyList<Move> PrincipalVariation`.
- [ ] Negamax with alpha-beta pruning. Simple move ordering:
  captures first (MVV-LVA is enough), then the rest in generation
  order. Checkmate score: `30000 - plyFromRoot`. Stalemate: 0.
- [ ] Iterative deepening: repeat at depth 1, 2, 3, … until the
  limits are exhausted; always return the completed deepest result.
  If time runs out mid-iteration, discard that partial iteration.
- [ ] Tests:
  - **Mate-in-1**: from
    `"6k1/5ppp/8/8/8/8/5PPP/R5K1 w - - 0 1"` (White to move, Ra8#)
    at depth 2, `BestMove.ToUci() == "a1a8"` and
    `ScoreCp >= 29000`.
  - **Mate-in-2**: a chosen mate-in-2 FEN — at depth 4 the search
    finds it (score ≥ 29000) and the PV has length ≥ 3.
  - **Avoid blunder**: from a position where the only non-losing
    move exists, depth-4 search returns that move.
  - **Determinism**: same position + same limits → same BestMove
    across two calls.
