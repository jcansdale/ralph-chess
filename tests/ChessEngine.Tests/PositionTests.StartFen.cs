namespace ChessEngine.Tests;

using Xunit;

public partial class PositionTests
{
    [Fact]
    public void StartFen_RoundTrips()
    {
        var start = Position.StartingPosition();
        Assert.Equal(Position.StartFen, start.ToFen());
    }

    [Fact]
    public void StartingPosition_HasCorrectSideToMove()
    {
        Assert.Equal(Color.White, Position.StartingPosition().SideToMove);
    }
}
