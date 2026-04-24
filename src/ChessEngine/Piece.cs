namespace ChessEngine;

/// <summary>
/// Represents the color of a piece.
/// </summary>
public enum Color : byte
{
    /// <summary>White color.</summary>
    White = 0,
    /// <summary>Black color.</summary>
    Black = 1
}

/// <summary>
/// Represents the type of a piece.
/// </summary>
public enum PieceType : byte
{
    /// <summary>No piece.</summary>
    None = 0,
    /// <summary>Pawn.</summary>
    Pawn = 1,
    /// <summary>Knight.</summary>
    Knight = 2,
    /// <summary>Bishop.</summary>
    Bishop = 3,
    /// <summary>Rook.</summary>
    Rook = 4,
    /// <summary>Queen.</summary>
    Queen = 5,
    /// <summary>King.</summary>
    King = 6
}

/// <summary>
/// Represents a piece with a specific color and type.
/// </summary>
public readonly struct Piece : IEquatable<Piece>
{
    /// <summary>The color of the piece.</summary>
    public Color Color { get; }

    /// <summary>The type of the piece.</summary>
    public PieceType Type { get; }

    /// <summary>Initializes a new instance of the <see cref="Piece"/> struct.</summary>
    /// <param name="color">The color of the piece.</param>
    /// <param name="type">The type of the piece.</param>
    public Piece(Color color, PieceType type)
    {

        Color = color;
        Type = type;
    }

    /// <summary>Represents a piece of no type (None).</summary>
    public static readonly Piece None = new Piece(Color.White, PieceType.None);

    /// <summary>Converts a char to a Piece.</summary>
    /// <param name="c">The piece character (P, N, B, R, Q, K for White; p, n, b, r, q, k for Black).</param>
    /// <returns>The piece represented by the character.</returns>
    /// <exception cref="ArgumentException">Thrown if the character does not represent a valid piece.</exception>
    public static Piece FromChar(char c)
    {
        return c switch
        {
            'P' => new Piece(Color.White, PieceType.Pawn),
            'N' => new Piece(Color.White, PieceType.Knight),
            'B' => new Piece(Color.White, PieceType.Bishop),
            'R' => new Piece(Color.White, PieceType.Rook),
            'Q' => new Piece(Color.White, PieceType.Queen),
            'K' => new Piece(Color.White, PieceType.King),
            'p' => new Piece(Color.Black, PieceType.Pawn),
            'n' => new Piece(Color.Black, PieceType.Knight),
            'b' => new Piece(Color.Black, PieceType.Bishop),
            'r' => new Piece(Color.Black, PieceType.Rook),
            'q' => new Piece(Color.Black, PieceType.Queen),
            'k' => new Piece(Color.Black, PieceType.King),
            _ => throw new ArgumentException($"Invalid piece character: {c}")
        };
    }

    /// <summary>Converts a piece to a char.</summary>
    /// <param name="piece">The piece to convert.</param>
    /// <returns>The piece character.</returns>
    public static char ToChar(Piece piece)
    {
        if (piece.Type == PieceType.None)
        {
            throw new InvalidOperationException("Cannot convert None piece to char.");
        }

        char baseChar = piece.Color == Color.White ? 
            piece.Type switch {
                PieceType.Pawn => 'P',
                PieceType.Knight => 'N',
                PieceType.Bishop => 'B',
                PieceType.Rook => 'R',
                PieceType.Queen => 'Q',
                PieceType.King => 'K',
                _ => throw new InvalidOperationException()
            } :
            piece.Type switch {
                PieceType.Pawn => 'p',
                PieceType.Knight => 'n',
                PieceType.Bishop => 'b',
                PieceType.Rook => 'r',
                PieceType.Queen => 'q',
                PieceType.King => 'k',
                _ => throw new InvalidOperationException()
            };
        return baseChar;
    }

    /// <inheritdoc />
    public bool Equals(Piece other) => Color == other.Color && Type == other.Type;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Piece p && Equals(p);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Color, Type);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(Piece left, Piece right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Piece left, Piece right) => !left.Equals(right);
}
