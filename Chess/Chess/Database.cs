using System;
using Entities;
using Enums;

namespace Database
{
    public interface DatabaseInterface
    {
    	GameStateEntity GetState();
    	void ResetBoard();
    	void SaveState(GameStateEntity gameState);
    }

    public class Database: DatabaseInterface
    {
        private GamePiece[][] board = new GamePiece[][] {
            new GamePiece[] {new GamePiece(PieceType.Bishop, Player.White), new GamePiece(PieceType.None, Player.None), new GamePiece(PieceType.None, Player.None), new GamePiece(PieceType.None, Player.None)},
            new GamePiece[] {new GamePiece(PieceType.None, Player.None), new GamePiece(PieceType.None, Player.None), new GamePiece(PieceType.Queen, Player.White), new GamePiece(PieceType.None, Player.None)},
            new GamePiece[] {new GamePiece(PieceType.Rook, Player.Black), new GamePiece(PieceType.None, Player.None), new GamePiece(PieceType.None, Player.None), new GamePiece(PieceType.None, Player.None)},
            new GamePiece[] {new GamePiece(PieceType.None, Player.None), new GamePiece(PieceType.Pawn, Player.White), new GamePiece(PieceType.None, Player.None), new GamePiece(PieceType.None, Player.None)}
        };

        public GameStateEntity GetState()
        {
            return new GameStateEntity(board);
        }

        public void ResetBoard()
        {
        }

        public void SaveState(GameStateEntity state)
        {
            board = state.GameBoard;
        }
    }
}
