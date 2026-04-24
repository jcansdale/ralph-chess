namespace ChessEngine;

public sealed partial class Position
{
    /// <summary>
    /// Standard chess starting position in FEN notation.
    /// </summary>
    public const string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    /// <summary>
    /// Creates a Position representing the standard chess starting position.
    /// </summary>
    public static Position StartingPosition() => FromFen(StartFen);
}
