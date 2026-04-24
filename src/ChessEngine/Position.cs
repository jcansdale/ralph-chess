using System.Text;

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

    /// <summary>
    /// Emits the FEN string for the current position.
    /// </summary>
    /// <returns>The FEN string.</returns>
    public string ToFen()
    {
        var fen = new StringBuilder();

        for (int rank = 7; rank >= 0; rank--)
        {
            int emptySquares = 0;
            for (int file = 0; file <= 7; file++)
            {
                int index = (rank << 3) | file;
                Piece piece = Board[index];
                if (piece.Type == PieceType.None)
                {
                    emptySquares++;
                }
                else
                {
                    if (emptySquares > 0)
                    {
                        fen.Append(emptySquares);
                        emptySquares = 0;
                    }
                    fen.Append(Piece.ToChar(piece));
                }
            }
            if (emptySquares > 0)
            {
                fen.Append(emptySquares);
            }
            if (rank > 0)
            {
                fen.Append('/');
            }
        }

        fen.Append(' ');
        fen.Append(SideToMove == Color.White ? "w" : "b");
        fen.Append(' ');

        string castling = "";
        if (CastlingRights.HasFlag(CastlingRights.WhiteKingside)) castling += 'K';
        if (CastlingRights.HasFlag(CastlingRights.WhiteQueenside)) castling += 'Q';
        if (CastlingRights.HasFlag(CastlingRights.BlackKingside)) castling += 'k';
        if (CastlingRights.HasFlag(CastlingRights.BlackQueenside)) castling += 'q';
        fen.Append(string.IsNullOrEmpty(castling) ? "-" : castling);
        fen.Append(' ');

        fen.Append(EnPassantSquare.HasValue ? EnPassantSquare.Value.ToString() : "-");
        fen.Append(' ');

        fen.Append(HalfmoveClock);
        fen.Append(' ');

        fen.Append(FullmoveNumber);

        return fen.ToString();
    }
}
