using System;
using System.Collections.Generic;
using Rules;
using Entities;
using Enums;
using Database;


namespace Chess
{
	public class GameLogic
	{
		private DatabaseInterface database;
		private RuleBook ruleBook;

		public GameLogic(DatabaseInterface _database, RuleBook _rules)
		{
			database = _database;
			ruleBook = _rules;
		}

		public GameStateEntity MovePiece(GameMoveEntity piece)
		{
			var gameState = database.GetState ();

			if (ruleBook.MoveIsValid(piece, gameState)) 
			{
				ExecuteMove (piece, gameState);
				database.SaveState (gameState);
				return database.GetState ();
			}

			return gameState;
		}

		private void ExecuteMove(GameMoveEntity piece, GameStateEntity state)
		{
            bool hasMoved = true;
            var movedPiece = state.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X];
            var target = state.GameBoard[piece.RequestedPos.Y] [piece.RequestedPos.X];

            if (Castling(movedPiece, target))
                PerformCastling(piece, state);
            else
            {
                state.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X] = new GamePiece(PieceType.None, Color.None);
                state.GameBoard[piece.RequestedPos.Y] [piece.RequestedPos.X] = new GamePiece(piece.Type, piece.Color, hasMoved);
            }
          
			state.ActivePlayer = state.ActivePlayer == Color.White ? Color.Black : Color.White;
		}

        private bool Castling(GamePiece movedPiece, GamePiece target)
        {
            return movedPiece.Type == PieceType.King &&
                   target.Type == PieceType.Rook &&
                   movedPiece.Color == target.Color;
        }

        private void PerformCastling(GameMoveEntity piece, GameStateEntity state)
        {
            var hasMoved = true;
            if (piece.Color == Color.White)
            {
                if (piece.RequestedPos.X == 7)
                {                   
                    state.GameBoard[7][7] = new GamePiece(PieceType.None, Color.None);
                    state.GameBoard[7][4] = new GamePiece(PieceType.None, Color.None);
                    state.GameBoard[7][6] = new GamePiece(PieceType.King, Color.White, hasMoved);
                    state.GameBoard[7][5] = new GamePiece(PieceType.Rook, Color.White, hasMoved);
                }
                else
                {
                    state.GameBoard[7][0] = new GamePiece(PieceType.None, Color.None);
                    state.GameBoard[7][4] = new GamePiece(PieceType.None, Color.None);
                    state.GameBoard[7][2] = new GamePiece(PieceType.King, Color.White, hasMoved);
                    state.GameBoard[7][3] = new GamePiece(PieceType.Rook, Color.White, hasMoved);
                }
            }
            else
            {
                if (piece.RequestedPos.X == 7)
                {
                    state.GameBoard[0][7] = new GamePiece(PieceType.None, Color.None);
                    state.GameBoard[0][3] = new GamePiece(PieceType.None, Color.None);
                    state.GameBoard[0][5] = new GamePiece(PieceType.King, Color.Black, hasMoved);
                    state.GameBoard[0][4] = new GamePiece(PieceType.Rook, Color.Black, hasMoved);
                }
                else
                {
                    state.GameBoard[0][0] = new GamePiece(PieceType.None, Color.None);
                    state.GameBoard[0][3] = new GamePiece(PieceType.None, Color.None);
                    state.GameBoard[0][1] = new GamePiece(PieceType.King, Color.Black, hasMoved);
                    state.GameBoard[0][2] = new GamePiece(PieceType.Rook, Color.Black, hasMoved);
                }
            }
        }

		public GameStateEntity TransformPiece(GameMoveEntity piece)
		{
			var gameState = database.GetState ();
            gameState.GameBoard [piece.RequestedPos.Y] [piece.RequestedPos.X] = new GamePiece(piece.Type, piece.Color);
			database.SaveState(gameState);

			return gameState;
		}


	}
}

