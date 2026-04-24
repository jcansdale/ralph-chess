using System.Text;

namespace ChessEngine;

/// <summary>
/// Represents the state of a chess game.
/// </summary>
public sealed partial class Position
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
        if (CastlingRights.HasFlag(CastlingRights.BlackKingside)) castling
            += 'k';
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

    /// <summary>
    /// Parses a FEN string into a <see cref="Position"/>.
    /// </summary>
    /// <param name="fen">The FEN string.</param>
    /// <returns>The parsed <see cref="Position"/>.</returns>
    /// <exception cref="FormatException">Thrown if the FEN string is invalid.</exception>
    public static Position FromFen(string fen)
    {
        string[] parts = fen.Split(' ');
        if (parts.Length != 6)
        {
            throw new FormatException("Invalid FEN string: expected 6 parts.");
        }

        Piece[] board = new Piece[64];
        for (int i = 0; i < 64; i++) board[i] = Piece.None;

        string[] ranks = parts[0].Split('/');
        if (ranks.Length != 8)
        {
            throw new FormatException("Invalid FEN string: expected 8 ranks.");
        }

        for (int rankIdx = 0; rankIdx < 8; rankIdx++)
        {
            int rank = 7 - rankIdx;
            string rankString = ranks[rankIdx];
            int file = 0;
            int emptySquares = 0;

            foreach (char c in rankString)
            {
                if (char.IsDigit(c))
                {
                    emptySquares += (c - '0');
                }
                else
                {
                    if (emptySquares > 0)
                    {
                        for (int i = 0; i < emptySquares; i++)
                        {
                            board[(rank << 3) | file] = Piece.None;
                            file++;
                        }
                        emptySquares = 0;
                    }
                    board[(rank << 3) | file] = Piece.FromChar(c);
                    file++;
                }
            }
            if (emptySquares > 0)
            {
                for (int i = 0; i < emptySquares; i++)
                {
                    board[(rank << 3) | file] = Piece.None;
                    file++;
                }
            }
            if (file != 8)
            {
                throw new FormatException("Invalid FEN string: rank does not have 8 squares.");
            }
        }

        Color side = parts[1] == "w" ? Color.White : (parts[1] == "b" ? Color.Black : throw new FormatException("Invalid FEN side to move."));

        CastlingRights castling = CastlingRights.None;
        if (parts[2] != "-")
        {
            foreach (char c in parts[2])
            {
                if (c == 'K') castling |= CastlingRights.WhiteKingside;
                else if (c == 'Q') castling |= CastlingRights.WhiteQueenside;
                else if (c == 'k') castling |= CastlingRights.BlackKingside;
                else if (c == 'q') castling |= CastlingRights.BlackQueenside;
                else throw new FormatException("Invalid FEN castling rights.");
            }
        }

        Square? enPassant = null;
        if (parts[3] != "-")
        {
            enPassant = Square.Parse(parts[3]);
        }

        int halfmove = int.Parse(parts[4]);
        int fullmove = int.Parse(parts[5]);

        return new Position(board, side, castling, enPassant, halfmove, fullmove);
    }
}
