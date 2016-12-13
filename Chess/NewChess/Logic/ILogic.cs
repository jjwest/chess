using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Logic
{
    public interface ILogic
    {
        GameStateEntity GetInitialState();
        GameStateEntity MovePiece(GameMoveEntity movement);

        GameStateEntity TransformPiece(GameMoveEntity movement);

        void ResetBoard();
    }
}
