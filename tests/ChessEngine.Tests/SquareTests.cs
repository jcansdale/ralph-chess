using ChessEngine;
using Xunit;

namespace ChessEngine.Tests;

public class SquareTests
{
    [Fact]
    public void AllSixtyFourSquares_RoundTripFileRankAndAlgebraic()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                var sq = Square.FromFileRank(file, rank);
                int expectedIndex = rank * 8 + file;
                Assert.Equal(expectedIndex, sq.Index);
                Assert.Equal(file, sq.File);
                Assert.Equal(rank, sq.Rank);

                string algebraic = $"{(char)('a' + file)}{(char)('1' + rank)}";
                Assert.Equal(algebraic, sq.ToString());

                var parsed = Square.Parse(algebraic);
                Assert.Equal(sq, parsed);
                Assert.Equal(expectedIndex, parsed.Index);
            }
        }
    }

    [Fact]
    public void KnownCorners_HaveExpectedIndices()
    {
        Assert.Equal(0, Square.FromFileRank(0, 0).Index);   // a1
        Assert.Equal(7, Square.FromFileRank(7, 0).Index);   // h1
        Assert.Equal(56, Square.FromFileRank(0, 7).Index);  // a8
        Assert.Equal(63, Square.FromFileRank(7, 7).Index);  // h8
        Assert.Equal("a1", new Square(0).ToString());
        Assert.Equal("h1", new Square(7).ToString());
        Assert.Equal("a8", new Square(56).ToString());
        Assert.Equal("h8", new Square(63).ToString());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(64)]
    public void Constructor_RejectsOutOfRange(int index)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Square(index));
    }

    [Fact]
    public void FromFileRank_RejectsOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Square.FromFileRank(-1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Square.FromFileRank(8, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Square.FromFileRank(0, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Square.FromFileRank(0, 8));
    }

    [Fact]
    public void Parse_RejectsMalformed()
    {
        Assert.Throws<FormatException>(() => Square.Parse("e9"));
        Assert.Throws<FormatException>(() => Square.Parse("i1"));
        Assert.Throws<FormatException>(() => Square.Parse("e"));
    }
}
