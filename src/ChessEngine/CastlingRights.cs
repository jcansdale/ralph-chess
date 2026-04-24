namespace ChessEngine;

/// <summary>
/// Represents the rights to castle.
/// </summary>
[System.Flags]
public enum CastlingRights : byte
{
    /// <summary>No castling rights.</summary>
    None = 0,
    /// <summary>White can castle kingside.</summary>
    WhiteKingside = 1 << 0,
    /// <summary>White can castle queenside.</summary>
    WhiteQueenside = 1 << 1,
    /// <summary>Black can castle kingside.</summary>
    BlackKingside = 1 << 2,
    /// <summary>Black can castle queenside.</summary>
    BlackQueenside = 1 << 3
}
