using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entities;
using Utility;

namespace Rules
{
    public interface Rule
    {
        bool IsValid(GameMoveEntity movement, GameStateEntity gameBoard);
    }
    public class OnlyMoveOwnPiece : Rule
    {
        public bool IsValid(GameMoveEntity movement, GameStateEntity state)
        {
            return movement.Color == state.ActivePlayer;
        }
    }
    public class RookMovement : Rule
    {
        public bool IsValid(GameMoveEntity movement, GameStateEntity state)
        {
            if (!movement.Type.Equals(PieceType.Rook))
                return true;
            if (Utilities.StepOnOwnPiece(movement, state))
                return false;
            return Utilities.IsLinear(movement, state) && Utilities.PathIsClear(movement, state.GameBoard);
        }
    }
    public class PawnMovement : Rule
    {
        public bool IsValid(GameMoveEntity movement, GameStateEntity state)
        {
            if (movement.Type != PieceType.Pawn)
                return true;
            if (Utilities.StepOnOwnPiece(movement, state))
                return false;
            return (NormalMovement(movement, state) ||
                    ChargeMovement(movement, state) ||
                    AttackMovement(movement, state));
        }
        private bool NormalMovement(GameMoveEntity movement, GameStateEntity state)
        {
            bool destinationEmpty = state.GameBoard.GetPieceAt(movement.RequestedPos).Type == PieceType.None;
            int deltaX = movement.RequestedPos.X - movement.CurrentPos.X;
            int deltaY = movement.RequestedPos.Y - movement.CurrentPos.Y;

            if (!destinationEmpty || deltaX != 0)
                return false;
            if (movement.Color == Color.White)
                return deltaY == -1;
            else
                return deltaY == 1;
        }
        private bool ChargeMovement(GameMoveEntity movement, GameStateEntity state)
        {
            int deltaX = Math.Abs(movement.RequestedPos.X - movement.CurrentPos.X);
            int deltaY = movement.RequestedPos.Y - movement.CurrentPos.Y;
            var gamePiece = state.GameBoard.GetPieceAt(movement.CurrentPos);
            bool destinationEmpty = state.GameBoard.GetPieceAt(movement.RequestedPos).Type == PieceType.None;

            if (deltaX != 0 || !Utilities.PathIsClear(movement, state.GameBoard) || !destinationEmpty)
                return false;
            if (movement.Color == Color.White)
                return deltaX == 0 && deltaY == -2 && !gamePiece.HasMoved;
            else
                return deltaX == 0 && deltaY == 2 && !gamePiece.HasMoved;
        }
        private bool AttackMovement(GameMoveEntity movement, GameStateEntity state)
        {
            int deltaX = Math.Abs(movement.RequestedPos.X - movement.CurrentPos.X);
            int deltaY = movement.RequestedPos.Y - movement.CurrentPos.Y;
            var target = state.GameBoard.GetPieceAt(movement.RequestedPos);

            if (movement.Color == Color.White)
                return deltaX == 1 && deltaY == -1 && target.Color == Color.Black;
            else
                return deltaX == 1 && deltaY == 1 && target.Color == Color.White;
        }
    }
    public class BishopMovement : Rule
    {
        public bool IsValid(GameMoveEntity movement, GameStateEntity state)
        {
            if (!movement.Type.Equals(PieceType.Bishop))
                return true;
            if (Utilities.StepOnOwnPiece(movement, state))
                return false;
            return Utilities.IsDiagonal(movement, state) && Utilities.PathIsClear(movement, state.GameBoard);
        }
    }
    public class KnightMovement : Rule
    {
        public bool IsValid(GameMoveEntity movement, GameStateEntity state)
        {
            if (!movement.Type.Equals(PieceType.Knight))
                return true;
            if (Utilities.StepOnOwnPiece(movement, state))
                return false;

            int deltaX = Math.Abs(movement.RequestedPos.X - movement.CurrentPos.X);
            int deltaY = Math.Abs(movement.RequestedPos.Y - movement.CurrentPos.Y);

            return deltaX == 2 && deltaY == 1 || deltaX == 1 && deltaY == 2;
        }
    }
    public class QueenMovement : Rule
    {
        public bool IsValid(GameMoveEntity movement, GameStateEntity state)
        {
            if (!movement.Type.Equals(PieceType.Queen))
                return true;
            if (Utilities.StepOnOwnPiece(movement, state))
                return false;

            return ((Utilities.IsLinear(movement, state) || Utilities.IsDiagonal(movement, state)) &&
            Utilities.PathIsClear(movement, state.GameBoard));
        }
    }
    public class KingMovement : Rule
    {
        public bool IsValid(GameMoveEntity movement, GameStateEntity state)
        {
            if (!movement.Type.Equals(PieceType.King))
                return true;

            return CastleMovement(movement, state) || NormalMovement(movement, state);
        }
        private bool CastleMovement(GameMoveEntity movement, GameStateEntity state)
        {
            var king = state.GameBoard.GetPieceAt(movement.CurrentPos);
            var rook = state.GameBoard.GetPieceAt(movement.RequestedPos);
            Point newKingPos = movement.RequestedPos;

            if (movement.RequestedPos.X == 0 && movement.RequestedPos.Y == 0)
                newKingPos = new Point(1, 0);
            else if (movement.RequestedPos.X == 7 && movement.RequestedPos.Y == 0)
                newKingPos = new Point(5, 0);
            else if (movement.RequestedPos.X == 0 && movement.RequestedPos.Y == 7)
                newKingPos = new Point(2, 7);
            else if (movement.RequestedPos.X == 7 && movement.RequestedPos.Y == 7)
                newKingPos = new Point(6, 7);

            GameStateEntity mockState = state.Clone();
            mockState.GameBoard.PlacePieceAt(newKingPos, mockState.GameBoard.GetPieceAt(movement.CurrentPos));
            mockState.GameBoard.PlacePieceAt(movement.CurrentPos, new GamePiece(PieceType.None, Color.None));

            return !king.HasMoved &&
                   !rook.HasMoved &&
                    rook.Type == PieceType.Rook &&
                    Utilities.PathIsClear(movement, state.GameBoard) &&
                    Utilities.StepOnOwnPiece(movement, state) &&
                   !Utilities.CheckedAfterCastling(mockState, mockState.ActivePlayer);
        }
        private bool NormalMovement(GameMoveEntity movement, GameStateEntity state)
        {
            if (Utilities.StepOnOwnPiece(movement, state))
                return false;

            int deltaX = Math.Abs(movement.RequestedPos.X - movement.CurrentPos.X);
            int deltaY = Math.Abs(movement.RequestedPos.Y - movement.CurrentPos.Y);
            return deltaX < 2 && deltaY < 2;
        }
    }
}