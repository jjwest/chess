using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Logic.Rules
{
    public class OnlyMoveOwnPiece : Rule
    {
        public bool IsValid(GameMoveEntity movement, GameStateEntity state)
        {
            return movement.Color == state.ActivePlayer;
        }
    }
}
