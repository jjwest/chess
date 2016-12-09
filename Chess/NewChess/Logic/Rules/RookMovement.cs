using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Logic.Rules
{
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
}
