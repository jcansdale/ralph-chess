namespace ChessEngine;

/// <summary>
/// Represents the state of a chess game.
/// </summary>
public sealed class Position
{
    /// <summary>
    /// The pieces on the board.
    /// </summary>
    public Piece[] Board { get; }

    /// <summary>
    /// The side to move.
    /// </summary>
    public Color SideToMove { get; }

    /// <summary>
    /// The castling rights.
    /// </summary>
    public CastlingRights CastlingRights { get; }

    /// <summary>
    /// The en-passant target square, if any.
    /// </summary>
    public Square? EnPassantSquare { get; }

    /// <summary>
    /// The halfmove clock.
    /// </summary>
    public int HalfmoveClock { get; }

    /// <summary>
        /// The fullmove number.
    /// </summary>
    public int FullmoveNumber { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> class.
    /// </summary>
    /// <param name="board">The board state.</param>
    /// <param name="sideToMove">The side to move.</param>
    /// <param name="castlingRights">The castling rights.</param>
    /// <param name="enPassantSquare">The en-passpute target square.</param>
    /// <param name="halfmoveClock">The halfmove clock.</param>
    /// <param name="fullmoveNumber">The fullmove number.</param>
    public Position(
        Piece[] board,
        Color sideToMove,
        CastlingRights castlingRights,
        Square? enPassantSquare,
        int halfmoveClock,
        int fullmoveNumber)
    {
        Board = new Piece[64];
        Array.Copy(board, Board, 64);
        SideToMove = sideToMove;
        CastlingRights = castlingRights;
        EnPassantSquare = enPassantSquare;
        HalfmoveClock = halfmoveClock;
        FullmoveNumber = fullmoveNumber;
    }

    /// <summary>
    /// Creates a deep copy of the current position.
    /// </summary>
    /// <returns>A new <see cref="Position"/> instance representing the same state.</returns>
    public Position Clone()
    {
        return new Position(Board, SideToMove, CastlingRights, EnPassantSquare, HalfmoveClock, FullmoveNumber);
    }
}
