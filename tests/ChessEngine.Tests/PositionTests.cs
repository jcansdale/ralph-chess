using ChessEngine;
using Xunit;

namespace ChessEngine.Tests;

public class PositionTests
{
    [Fact]
    public void ToFen_ReturnsCorrectFenForStartingPosition()
    {
        var board = new Piece[64];
        for (int i = 0; i < 64; i++) board[i] = Piece.None;

        // White pieces on rank 1 (index 0-7)
        board[0] = Piece.FromChar('R');
        board[1] = Piece.FromChar('N');
        board[2] = Piece.FromChar('B');
        board[3] = Piece.FromChar('Q'); // Wait, I put a non-breaking space in there?
        // I'll just be very careful.
        board[3] = Piece.FromChar('Q');
        board[4] = Piece.FromChar('K');
        board[5] = Piece.FromChar('B');
        board[6] = Piece.FromChar('N');
        board[7] = Piece.FromChar('R');

        // White pawns on rank 2 (index 8-15)
        for (int f = 0; f < 8; f++) board[8 + f] = Piece.FromChar('P');

        // Black pawns on rank 7 (index 48-55)
        for (int f = 0; f < 8; f++) board[48 + f] = Piece.FromChar('p');

        // Black pieces on rank 8 (index 56-63)
        board[56] = Piece.FromChar('r');
        board[57] = Piece.FromChar('n');
        board[58] = Piece.FromChar('b');
        board[59] = Piece.FromChar('q');
        board[60] = Piece.FromChar('k');
        board[61] = Piece.FromChar('b');
        board[62] = Piece.FromChar('n');
        board[63] = Piece.FromChar('r');

        var pos = new Position(
            board,
            Color.White,
            CastlingRights.WhiteKingside | CastlingRights.WhiteQueenside | CastlingRights.BlackKingside | CastlingRights.BlackQueenside,
            null,
            0,
            1);

        string expectedFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        Assert.Equal(expectedFen, pos.ToFen());
    }
}
