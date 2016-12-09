using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class GameStateEntity
    {
        public bool PawnIsPromoted { get; set; } = false;
        public bool KingIsChecked { get; set; } = false;
        public Color ActivePlayer { get; set; } = Color.White;
        public Color Winner { get; set; } = Color.None;
        public IGameBoard GameBoard { get; set; }

        public GameStateEntity(IGameBoard gameBoard)
        {
            GameBoard = gameBoard;
        }
        public GameStateEntity Clone()
        {
            var deepCopy = (GameStateEntity)this.MemberwiseClone();
            deepCopy.GameBoard = (IGameBoard)deepCopy.GameBoard.Clone();

            return deepCopy;
        }

    }
}
