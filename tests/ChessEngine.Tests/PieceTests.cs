using ChessEngine;
using Xunit;

namespace ChessEngine.Tests;

public class PieceTests
{
    [Theory]
    [InlineData('P', Color.White, PieceType.Pawn)]
    [InlineData('N', Color.White, PieceType.Knight)] // wait, typo in my thought, checking text
    [InlineData('B', Color.White, PieceType.Bishop)]
    [InlineData('R', Color.White, PieceType.Rook)]
    [InlineData('Q', Color.White, PieceType.Queen)]
    [InlineData('K', Color.White, PieceType.King)]
    [InlineData('p', Color.Black, PieceType.Pawn)]
    [InlineData('n', Color.Black, PieceType.Knight)]
    [InlineData('b', Color.Black, PieceType.Bishop)]
    [InlineData('r', Color.Black, PieceType.Rook)]
    [InlineData('q', Color.Black, PieceType.Queen)]
    [InlineData('k', Color.Black, PieceType.King)]
    public void FromChar_RoundTrip(char c, Color color, PieceType type)
    {
        var piece = Piece.FromChar(c);
        Assert.Equal(color, piece.Color);
        Assert.Equal(type, piece.Type);
        Assert.Equal(c, Piece.ToChar(piece));
    }

    [Fact]
    public void FromChar_Invalid_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Piece.FromChar('X'));
        Assert.Throws<ArgumentException>(() => Piece.FromChar('1'));
    }

    [Fact]
    public void ToChar_None_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => Piece.ToChar(Piece.None));
    }

    [Fact]
    public void Equality_Works()
    {
        var p1 = new Piece(Color.White, PieceType.Pawn);
        var p2 = new Piece(Color.White, PieceType.Pawn);
        var p3 = new Piece(Color.Black, PieceType.Pawn);

        Assert.Equal(p1, p2);
        Assert.NotEqual(p1, p3);
        Assert.True(p1 == p2);
        Assert.True(p1 != p3);
    }
}
