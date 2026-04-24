namespace ChessEngine;

/// <summary>
/// A board square, indexed 0..63 using little-endian rank-file mapping
/// (0 = a1, 7 = h1, 56 = a8, 63 = h8).
/// </summary>
public readonly struct Square : IEquatable<Square>
{
    private readonly byte _index;

    /// <summary>Creates a square from its 0..63 index.</summary>
    /// <param name="index">Little-endian rank-file index (0 = a1, 63 = h8).</param>
    public Square(int index)
    {
        if ((uint)index > 63)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Square index must be in 0..63.");
        }
        _index = (byte)index;
    }

    /// <summary>The raw 0..63 index of this square.</summary>
    public int Index => _index;

    /// <summary>The file (0 = a … 7 = h).</summary>
    public int File => _index & 7;

    /// <summary>The rank (0 = rank 1 … 7 = rank 8).</summary>
    public int Rank => _index >> 3;

    /// <summary>Constructs a square from file (0..7) and rank (0..7).</summary>
    public static Square FromFileRank(int file, int rank)
    {
        if ((uint)file > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(file), file, "File must be in 0..7.");
        }
        if ((uint)rank > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(rank), rank, "Rank must be in 0..7.");
        }
        return new Square((rank << 3) | file);
    }

    /// <summary>Parses a two-character algebraic square like "e4".</summary>
    public static Square Parse(string algebraic)
    {
        ArgumentNullException.ThrowIfNull(algebraic);
        if (algebraic.Length != 2)
        {
            throw new FormatException($"Invalid square '{algebraic}'.");
        }
        char f = algebraic[0];
        char r = algebraic[1];
        if (f < 'a' || f > 'h' || r < '1' || r > '8')
        {
            throw new FormatException($"Invalid square '{algebraic}'.");
        }
        return FromFileRank(f - 'a', r - '1');
    }

    /// <summary>Returns the algebraic name of the square, e.g. "e4".</summary>
    public override string ToString()
    {
        return string.Create(2, _index, static (span, idx) =>
        {
            span[0] = (char)('a' + (idx & 7));
            span[1] = (char)('1' + (idx >> 3));
        });
    }

    /// <inheritdoc />
    public bool Equals(Square other) => _index == other._index;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Square s && Equals(s);

    /// <inheritdoc />
    public override int GetHashCode() => _index;

    /// <summary>Equality operator.</summary>
    public static bool operator ==(Square left, Square right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Square left, Square right) => !left.Equals(right);
}
