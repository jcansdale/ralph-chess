namespace ChessEngine;

/// <summary>
/// Represents the flags for a move.
/// </summary>
[Flags]
public enum MoveFlags : byte
{
    /// <summary>No flags.</summary>
    None = 0,
    /// <summary>The move is a double pawn push.</summary>
    DoublePush = 1 << 0,
    /// <summary>The move is an en passant capture.</summary>
    EnPassant = 1 << 1,
    /// <summary>The move is a capture.</summary>
    Capture = 1 << 2
}

/// <summary>
/// Represents a move on the chessboard.
/// </summary>
public readonly struct Move : IEquatable<Move>
{
    /// <summary>The starting square of the move.</summary>
    public readonly Square From { get; }

    /// <summary>The destination square of the move.</summary>
    public readonly Square To { get; }

    /// <summary>The piece type to promote to, if the move is a promotion (None otherwise).</summary>
    public readonly PieceType Promotion { get; }

    /// <summary>Flags indicating special properties of the move.</summary>
    public readonly MoveFlags Flags { get; }

    /// <summary>Initializes a new instance of the <see cref="Move"/> struct.</summary>
    /// <param name="from">The starting square.</param>
    /// <param name="to">The destination square.</param>
    /// <param name="promotion">The piece type to promote to (None if not a promotion).</param>
    /// <param name="flags">Flags for the move.</param>
    public Move(Square from, Square to, PieceType promotion = PieceType.None, MoveFlags flags = MoveFlags.None)
    {
        From = from;
        To = to;
        Promotion = promotion;
        Flags = flags;
    }

    /// <summary>
    /// Returns the UCI notation for this move.
    /// </summary>
    /// <returns>A string representing the move in UCI format (e.g., "e2e4", "e7e8q").</returns>
    public string ToUci()
    {
        string uci = $"{From}{To}";
        if (Promotion != PieceType.None)
        {
            char promoChar = Promotion switch
            {
                PieceType.Pawn => 'p', // Should not happen in UCI promotion, but for completeness
                PieceType.Knight => 'n',
                PieceType.Bishop => 'b',
                PieceType.Rook => 'r',
                PieceType.Queen => 'q',
                _ => throw new InvalidOperationException($"Invalid promotion type: {Promotion}")
            };
            uci += promoChar;
        }
        return uci;
    }

    /// <inheritdoc />
    public bool Equals(Move other) => From == other.From && To == other.To && Promotion == other.Promotion && Flags == other.Flags;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Move other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(From, To, Promotion, Flags);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(Move left, Move right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Move left, Move right) => !left.Equals(right);
}
