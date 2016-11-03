using System;
using Chess;

namespace Rules
{
	public interface Rule
	{
		bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard);
	}

	public class RookMovement : Rule
	{
		public bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard)
		{
			return true;
		}
	}
}

