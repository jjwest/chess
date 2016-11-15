using System;
using System.Linq;
using Enums;
using Entities;
using Utility;

namespace Rules
{
    public interface Rule
    {
        bool IsValid(GameMoveEntity piece, GameStateEntity gameBoard);
    }



    public class OnlyMoveOwnPiece : Rule
    {
        public bool IsValid(GameMoveEntity piece, GameStateEntity state)
        {
            return piece.Color == state.ActivePlayer;
        }
    }

    public class RookMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
            if (!piece.Type.Equals(PieceType.Rook))
                return true;
	        if (Utilities.StepOnOwnPiece(piece, state))
                return false;

            return Utilities.IsLinear(piece, state) && Utilities.PathIsClear(piece, state.GameBoard);
	}
    }

    public class PawnMovement : Rule
    {
    	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
    	{
                if (!piece.Type.Equals(PieceType.Pawn))
                    return true;
                if (Utilities.StepOnOwnPiece(piece, state))
                    return false;

                return (NormalMovement(piece, state) ||
    		            ChargeMovement(piece, state) ||
    		            AttackMovement(piece, state));
    	}

        private bool NormalMovement(GameMoveEntity piece, GameStateEntity state)
        {
            int deltaX = piece.RequestedPos.X - piece.CurrentPos.X;
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;

            if (deltaX != 0)
                return false;
            if (piece.Color == Color.White)
                return deltaY == -1;
            else
                return deltaY == 1;

        }

        private bool ChargeMovement(GameMoveEntity piece, GameStateEntity state)
        {
            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;
            var gamePiece = state.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X];

            if (piece.Color == Color.White)
                return deltaX == 0 && deltaY == -2 && !gamePiece.HasMoved;
            else
                return deltaX == 0 && deltaY == 2 && !gamePiece.HasMoved;
        }

        private bool AttackMovement(GameMoveEntity piece, GameStateEntity state)
        {
            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;
            var target = state.GameBoard[piece.RequestedPos.Y][piece.RequestedPos.X];

            if (piece.Color == Color.White)
                return deltaX == 1 && deltaY == -1 && target.Color == Color.Black;
            else
                return deltaX == 1 && deltaY == 1 && target.Color == Color.Black;
        }
    }

    public class BishopMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
	    if (!piece.Type.Equals(PieceType.Bishop))
		return true;
	    if (Utilities.StepOnOwnPiece(piece, state))
                return false;

            return Utilities.IsDiagonal(piece, state) && Utilities.PathIsClear(piece, state.GameBoard);
	}
    }

    public class KnightMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
            if (!piece.Type.Equals(PieceType.Knight))
                return true;
            if (Utilities.StepOnOwnPiece(piece, state))
                return false;

            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = Math.Abs(piece.RequestedPos.Y - piece.CurrentPos.Y);

            return deltaX == 2 && deltaY == 1 || deltaX == 1 && deltaY == 2;
	}
    }

    public class QueenMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
            if (!piece.Type.Equals(PieceType.Queen))
                return true;
            if (Utilities.StepOnOwnPiece(piece, state))
                return false;

            return ((Utilities.IsLinear(piece, state) || Utilities.IsDiagonal(piece, state)) &&
		    Utilities.PathIsClear(piece, state.GameBoard));
	}
    }


    public class KingMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
            if (!piece.Type.Equals(PieceType.King))
                return true;

            return CastleMovement(piece, state) || NormalMovement(piece, state);
	}

        private bool CastleMovement(GameMoveEntity piece, GameStateEntity state)
        {
            var king = state.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X];
            var rook = state.GameBoard[piece.RequestedPos.Y][piece.RequestedPos.X];
            Point newKingPos = piece.RequestedPos;

            if (piece.RequestedPos.X == 0 && piece.RequestedPos.Y == 0)
                newKingPos = new Point(1, 0);
            else if (piece.RequestedPos.X == 7 && piece.RequestedPos.Y == 0)
                newKingPos = new Point(5, 0);
            else if (piece.RequestedPos.X == 0 && piece.RequestedPos.Y == 7)
                newKingPos = new Point(2, 7);
            else if (piece.RequestedPos.X == 7 && piece.RequestedPos.Y == 7)
                newKingPos = new Point(6, 7);

            GameStateEntity mockState = new GameStateEntity(Utilities.DeepCopy(state.GameBoard));
            mockState.ActivePlayer = state.ActivePlayer;
            mockState.KingIsChecked = state.KingIsChecked;
            mockState.PawnIsPromoted = state.PawnIsPromoted;
            mockState.Winner = state.Winner;
            mockState.GameBoard[newKingPos.Y][newKingPos.X] = mockState.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X];
            mockState.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X] = new GamePiece(PieceType.None, Color.None);
        

            return !king.HasMoved &&
	           	   !rook.HasMoved &&
		            rook.Type == PieceType.Rook &&
		            Utilities.PathIsClear(piece, state.GameBoard) &&
		            Utilities.StepOnOwnPiece(piece, state) &&
                   !Utilities.KingIsChecked(mockState, mockState.ActivePlayer);
        }

        private bool NormalMovement(GameMoveEntity piece, GameStateEntity state)
        {
            if (Utilities.StepOnOwnPiece(piece, state))
                return false;

            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = Math.Abs(piece.RequestedPos.Y - piece.CurrentPos.Y);

            return deltaX < 2 && deltaY < 2;
        }
    }

    public class Check : Rule
    {
    	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
    	{
            GameStateEntity mockState = new GameStateEntity(Utilities.DeepCopy((state.GameBoard)));
            mockState.ActivePlayer = state.ActivePlayer;
            mockState.KingIsChecked = state.KingIsChecked;
            mockState.PawnIsPromoted = state.PawnIsPromoted;
            mockState.Winner = state.Winner;

            mockState.GameBoard[piece.RequestedPos.Y][piece.RequestedPos.X] = mockState.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X];
            mockState.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X] = new GamePiece(PieceType.None, Color.None);

            return !Utilities.KingIsChecked(mockState, state.ActivePlayer);
    	}
    }
}
