using System;
using Enums;
using Entities;

namespace Rules
{
	public interface Rule
	{
		bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard);
	}

	public class Movement
	{
		public static bool StepOnOwnPiece(GamePieceEntity piece, GameStateEntity gameState)
		{
			return gameState.GameBoard[piece.RequestedPos.y] [piece.RequestedPos.x].Item2 == piece.Color;
		}
	}

	public class RookMovement : Rule
	{
		public bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard)
		{
			if (!piece.Type.Equals(GamePieces.Rook)) { return true; }
			if (Movement.StepOnOwnPiece(piece, gameBoard)) { return false; }

			return (piece.RequestedPos.x == piece.CurrentPos.x || 
					piece.RequestedPos.y == piece.CurrentPos.y);
		}
	}
		
	public class PawnMovement : Rule
	{
		public bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard)
		{
			return true;
		}
	}

	public class BishopMovement : Rule
	{
		public bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard)
		{
			return true;
		}
	}

	public class KnightMovement : Rule
	{
		public bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard)
		{
			return true;
		}
	}

	public class QueenMovement : Rule
	{
		public bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard)
		{
			return true;
		}
	}

	public class KingMovement : Rule
	{
		public bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard)
		{
			return true;
		}
	}

	public class Check : Rule
	{
		public bool IsValid(GamePieceEntity piece, GameStateEntity gameBoard)
		{
			return true;
		}
	}
}

