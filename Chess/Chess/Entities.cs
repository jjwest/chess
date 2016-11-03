using System;

namespace Chess
{
	public struct Point
	{
		public int x;
		public int y;
	}
	
	public class GameStateEntity
	{
		public Tuple<GamePieces, Players> [][] GameBoard { get; set; } 
		public bool PawnIsPromoted { get; set; } = false;
		public bool KingIsChecked { get; set; } = false;
		public Players Winner { get; set; } = Players.None;

		GameStateEntity(Tuple<GamePieces, Players> [][] gameBoard)
		{
			GameBoard = gameBoard;
		}
	}

	public class GamePieceEntity
	{
		public Players Color { get; set; }
		public GamePieces Type { get; set; }
		public Point CurrentPos { get; set; }
		public Point RequestedPos { get; set; }

		public GamePieceEntity(GamePieces type, Point currentPos, Point requestedPos)
		{
			Type = type;
			CurrentPos = currentPos;
			RequestedPos = requestedPos;
		}
	}
}

