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

		GameLogic(DatabaseInterface _database)
		{
			database = _database;
			LoadRules();
		}

		public GameStateEntity MovePiece(GamePieceEntity piece)
		{
			var gameState = GetBoardState();

			if (ruleBook.MoveIsValid(piece, gameState)) 
			{
				gameState.GameBoard [piece.CurrentPos.y] [piece.CurrentPos.x] = Tuple.Create (GamePieces.None, Players.None);
				gameState.GameBoard [piece.RequestedPos.y] [piece.RequestedPos.x] = Tuple.Create (piece.Type, piece.Color);
				gameState.ActivePlayer = gameState.ActivePlayer == Players.White ? Players.Black : Players.White;
				UpdateBoard(gameState);
			}

			return gameState;
		}

		public GameStateEntity TransformPiece(GamePieceEntity piece)
		{
			var gameState = GetBoardState();
			gameState.GameBoard [piece.RequestedPos.y] [piece.RequestedPos.x] = Tuple.Create (piece.Type, piece.Color);
			UpdateBoard(gameState);

			return gameState;
		}

		public void ResetBoard()
		{
			database.ClearBoard();
		}

		private void LoadRules()
		{
			var rules = new List<Rule>();
			rules.Add (new RookMovement());
			ruleBook = new RuleBook (rules);
		}

		private void UpdateBoard(GameStateEntity gameBoard)
		{
			database.SetState(gameBoard);
		}

		private GameStateEntity GetBoardState()
		{
			return database.GetState();
		}

	}
}

