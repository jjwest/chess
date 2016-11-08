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
    	public static bool StepOnOwnPiece(GamePieceEntity piece, GameStateEntity state)
    	{
            return state.GameBoard[piece.RequestedPos.Y] [piece.RequestedPos.X].Color == piece.Color;
    	}

        public static bool IsLinear(GamePieceEntity piece, GameStateEntity state)
        {
            return (piece.RequestedPos.X == piece.CurrentPos.X ||
                    piece.RequestedPos.Y == piece.CurrentPos.Y);
        }

        public static bool IsDiagonal(GamePieceEntity piece, GameStateEntity state)
        {
            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = Math.Abs(piece.RequestedPos.Y - piece.CurrentPos.Y);

            return deltaX == deltaY;
        }

    	public static bool PathIsClear(GamePieceEntity piece, GameStateEntity state)
    	{
    	    int deltaX = piece.RequestedPos.X - piece.CurrentPos.X;
    	    int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;
    	    int stepX = deltaX == 0 ? 0 : deltaX / System.Math.Abs(deltaX);
    	    int stepY = deltaY == 0 ? 0 : deltaY / System.Math.Abs(deltaY);
            int currX = piece.CurrentPos.X;
            int currY = piece.CurrentPos.Y;

            for (int i = 1; i < Math.Max(deltaX, deltaY) - 1; i++)
    		{
                if ( !state.GameBoard[currY + i * stepY][currX + i * stepX].Equals(PieceType.None))
                    return false;
    	    }
    		
            return true;
    	}
    }

	public class RookMovement : Rule
	{
	    public bool IsValid(GamePieceEntity piece, GameStateEntity state)
	    {
            if (!piece.Type.Equals(PieceType.Rook)) 
                return true;
    		if (Movement.StepOnOwnPiece(piece, state))
                return false; 
    		
            return Movement.IsLinear(piece, state) && Movement.PathIsClear(piece, state);
	    }
	}

	public class PawnMovement : Rule
	{
	    public bool IsValid(GamePieceEntity piece, GameStateEntity state)
	    {
            if (!piece.Type.Equals(PieceType.Pawn)) 
                return true;
            if (Movement.StepOnOwnPiece(piece, state)) 
                return false; 
      
            return NormalMovement(piece, state) || 
                   ChargeMovement(piece, state) || 
                   AttackMovement(piece, state);
	    }

        private bool NormalMovement(GamePieceEntity piece, GameStateEntity state)
        {
            int deltaX = piece.RequestedPos.X - piece.CurrentPos.X;
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;

            if (deltaX != 0)
                return false;
            if (piece.Color == Player.White)
                return deltaY == -1;
            else
                return deltaY == 1;

        }

        private bool ChargeMovement(GamePieceEntity piece, GameStateEntity state)
        {
            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;
            var gamePiece = state.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X];

            if (piece.Color == Player.White)
                return deltaX == 0 && deltaY == -2 && !gamePiece.HasMoved;
            else
                return deltaX == 0 && deltaY == 2 && !gamePiece.HasMoved;
        }

        private bool AttackMovement(GamePieceEntity piece, GameStateEntity state)
        {
            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;
            var target = state.GameBoard[piece.RequestedPos.Y][piece.RequestedPos.X];

            if (piece.Color == Player.White)
                return deltaX == 1 && deltaY == -1 && target.Color == Player.Black;
            else
                return deltaX == 1 && deltaY == 1 && target.Color == Player.Black;
        }
	}

	public class BishopMovement : Rule
	{
	    public bool IsValid(GamePieceEntity piece, GameStateEntity state)
	    {
    		if (!piece.Type.Equals(PieceType.Bishop)) 
                return true;
    		if (Movement.StepOnOwnPiece(piece, state))
                return false;

            return Movement.IsDiagonal(piece, state) && Movement.PathIsClear(piece, state);
	    }
	}

	public class KnightMovement : Rule
	{
	    public bool IsValid(GamePieceEntity piece, GameStateEntity state)
	    {
            if (!piece.Type.Equals(PieceType.Knight)) 
                return true;
            if (Movement.StepOnOwnPiece(piece, state))
                return false;

            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = Math.Abs(piece.RequestedPos.Y - piece.CurrentPos.Y);

            return deltaX == 2 && deltaY == 1 || deltaX == 1 && deltaY == 2;
	    }
	}

	public class QueenMovement : Rule
	{
	    public bool IsValid(GamePieceEntity piece, GameStateEntity state)
	    {
            if (!piece.Type.Equals(PieceType.Queen)) 
                return true;
            if (Movement.StepOnOwnPiece(piece, state)) 
                return false;

            return ((Movement.IsLinear(piece, state) || Movement.IsDiagonal(piece, state)) &&
                     Movement.PathIsClear(piece, state));
	    }
	}


    // ROKAD INTE KLAR
	public class KingMovement : Rule
	{
	    public bool IsValid(GamePieceEntity piece, GameStateEntity state)
	    {
            if (!piece.Type.Equals(PieceType.King)) 
                return true;
            if (Movement.StepOnOwnPiece(piece, state)) 
                return false;

            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = Math.Abs(piece.RequestedPos.Y - piece.CurrentPos.Y);

            return deltaX < 2 && deltaY < 2;
	    }
	}

	public class Check : Rule
	{
	    public bool IsValid(GamePieceEntity piece, GameStateEntity state)
	    {
		return true;
	    }
	}
    }
