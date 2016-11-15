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
            new GamePiece[] {
                new GamePiece(PieceType.King, Color.Black), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.Rook, Color.White),
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None)
            },
            new GamePiece[] {
                new GamePiece(PieceType.Pawn, Color.Black), 
                new GamePiece(PieceType.Pawn, Color.Black), 
                new GamePiece(PieceType.Pawn, Color.Black), 
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None)
            },
            new GamePiece[] {
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None)
            },
            new GamePiece[] {
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None)
            },
            new GamePiece[] {
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None)
            },
            new GamePiece[] {
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None)
            },
            new GamePiece[] {
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None)
            },
            new GamePiece[] {
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None), 
                new GamePiece(PieceType.None, Color.None)
            }  
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
