using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Logic.Rules
{
    public interface Rule
    {
        bool IsValid(GameMoveEntity movement, GameStateEntity gameBoard);
    }
}
