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

		public GameStateEntity MovePiece(GamePieceEntity piece)
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

		private void ExecuteMove(GamePieceEntity piece, GameStateEntity state)
		{
            bool hasMoved = true;
            state.GameBoard [piece.CurrentPos.Y] [piece.CurrentPos.X] = new GamePiece(PieceType.None, Player.None);
            state.GameBoard [piece.RequestedPos.Y] [piece.RequestedPos.X] = new GamePiece(piece.Type, piece.Color, hasMoved);
			state.ActivePlayer = state.ActivePlayer == Player.White ? Player.Black : Player.White;

		}

		public GameStateEntity TransformPiece(GamePieceEntity piece)
		{
			var gameState = database.GetState ();
            gameState.GameBoard [piece.RequestedPos.Y] [piece.RequestedPos.X] = new GamePiece(piece.Type, piece.Color);
			database.SaveState(gameState);

			return gameState;
		}


	}
}

