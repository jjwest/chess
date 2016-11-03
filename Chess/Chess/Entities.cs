using System;
using Enums;

namespace Entities
{
	public struct Point
	{
		public int x;
		public int y;
	}
	
	public class GameStateEntity
	{
		public bool PawnIsPromoted { get; set; } = false;
		public bool KingIsChecked { get; set; } = false;
		public Players ActivePlayer { get; set; } = Players.White;
		public Players Winner { get; set; } = Players.None;
		public Tuple<GamePieces, Players> [][] GameBoard { get; set; } 

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

