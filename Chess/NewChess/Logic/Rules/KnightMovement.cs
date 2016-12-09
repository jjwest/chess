using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Logic.Rules
{
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
}
