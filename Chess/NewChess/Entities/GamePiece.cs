using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class GamePiece
    {
        public PieceType Type { get; set; }
        public Color Color { get; set; }
        public bool HasMoved { get; set; }

        public GamePiece() { }
        public GamePiece(PieceType piece, Color color)
        {
            Type = piece;
            Color = color;
            HasMoved = false;
        }
        public GamePiece(PieceType piece, Color color, bool hasMoved)
        {
            Type = piece;
            Color = color;
            HasMoved = hasMoved;
        }
    }
}
