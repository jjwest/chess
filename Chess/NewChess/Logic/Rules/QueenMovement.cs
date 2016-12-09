using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Logic.Rules
{
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
}
