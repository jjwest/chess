using System;
using System.Collections.Generic;
using Rules;


namespace Chess
{
	public class GameLogic
	{
		private RuleBook ruleBook;

		GameLogic()
		{
			LoadRules();
		}

		public GameStateEntity MovePiece(GamePieceEntity piece)
		{
			var gameState = GetBoardState();

			if (ruleBook.MoveIsValid(piece, gameState)) 
			{
				gameState.GameBoard [piece.CurrentPos.y] [piece.CurrentPos.x] = Tuple.Create (GamePieces.None, Players.None);
				gameState.GameBoard [piece.RequestedPos.y] [piece.RequestedPos.x] = Tuple.Create (piece.Type, piece.Color);
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
			//DB.ClearBoard();
		}

		private void LoadRules()
		{
			var rules = new List<Rule>();
			rules.Add (new RookMovement());
			ruleBook = new RuleBook (rules);
		}

		private void UpdateBoard(GameStateEntity gameBoard)
		{
			//DB.SetState();
		}

		private GameStateEntity GetBoardState()
		{
			//DB.GetState();
		}

	}
}

