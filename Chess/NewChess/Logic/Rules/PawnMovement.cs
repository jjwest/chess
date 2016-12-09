using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Logic.Rules
{
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
}
