# 040 — Sliding pieces: bishop, rook, queen

## Items

- [ ] Implement ray-based generation for bishops (4 diagonal rays),
  rooks (4 orthogonal rays), and queens (all 8). Each ray extends
  until it hits the board edge, an own piece (stop before it), or
  an opposing piece (include capture, then stop).
- [ ] Keep the implementation simple and correct: naive ray scanning
  is fine. Bitboard magic is explicitly out of scope.
- [ ] Tests:
  - From `"4k3/8/8/3B4/8/8/8/4K3 w - - 0 1"` the bishop has 13 moves.
  - From `"4k3/8/8/3R4/8/8/8/4K3 w - - 0 1"` the rook has 14 moves.
  - From `"4k3/8/8/3Q4/8/8/8/4K3 w - - 0 1"` the queen has 27 moves.
  - A rook blocked immediately by own pieces on all four sides has 0 moves.
  - A bishop with opposing pieces on two diagonals and own on the
    other two has exactly the capture squares reachable.
