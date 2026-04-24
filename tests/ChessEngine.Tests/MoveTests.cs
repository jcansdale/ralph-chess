using ChessEngine;
using Xunit;

namespace ChessEngine.Tests;

public class MoveTests
{
    [Fact]
    public void ToUci_QuietMove_ReturnsCorrectString()
    {
        var from = Square.Parse("e2");
        var to = Square.Parse("e4");
        var move = new Move(from, to);
        Assert.Equal("e2e4", move.ToUci());
    }

    [Fact]
    public void ToUci_DoublePush_ReturnsCorrectString()
    {
        var from = Square.Parse("e2");
        var to = Square.Parse("e4");
        var move = new Move(from, to, PieceType.None, MoveFlags.DoublePush);
        Assert.Equal("e2e4", move.ToUci());
    }

    [Fact]
    public void ToUci_EnPassant_ReturnsCorrectString()
    {
        var from = Square.Parse("e3");
        var to = Square.Parse("e4");
        var move = new Move(from, to, PieceType.None, MoveFlags.EnPassant);
        Assert.Equal("e3e4", move.ToUci());
    }

    [Fact]
    public void ToUci_Capture_ReturnsCorrectString()
    {
        var from = Square.Parse("e2");
        var CapTo = Square.Parse("e4");
        var move = new Move(from, CapTo, PieceType.None, MoveFlags.Capture);
        Assert.Equal("e2e4", move.ToUci());
    }

    [Theory]
    [InlineData(PieceType.Queen, "q")]
    [InlineData(PieceType.Rook, "r")]
    [InlineData(PieceType.Bishop, "b")]
    [InlineData(PieceType.Knight, "n")]
    public void ToUci_Promotion_ReturnsCorrectString(PieceType promotion, string promoChar)
    {
        var from = Square.Parse("e7");
        var to = Square.Parse("e8");
        var move = new Move(from, to, promotion);
        Assert.Equal($"e7e8{promoChar}", move.ToUci());
    }
}
