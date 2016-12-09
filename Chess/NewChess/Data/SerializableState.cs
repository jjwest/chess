using System;
using System.Collections.Generic;
using Entities;

namespace Data
{
    public class SerializableState
    {
        public bool PawnIsPromoted { get; set; }
        public bool KingIsChecked { get; set; }
        public Color ActivePlayer { get; set; }
        public Color Winner { get; set; }
        public List<GamePiece> GameBoard { get; set; }

        public SerializableState() { }

        public SerializableState(GameStateEntity state)
        {
            GameBoard = new List<GamePiece>();

            for (int y = 0; y < state.GameBoard.Width(); y++)
            {
                for (int x = 0; x < state.GameBoard.Width(); x++)
                {
                    GameBoard.Add(state.GameBoard.GetPieceAt(new Point(x, y)));
                }
            }

            PawnIsPromoted = state.PawnIsPromoted;
            KingIsChecked = state.KingIsChecked;
            ActivePlayer = state.ActivePlayer;
            Winner = state.Winner;
        }
    }
}

