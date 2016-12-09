using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class GameMoveEntity
    {
        public Color Color { get; set; }
        public PieceType Type { get; set; }
        public Point CurrentPos { get; set; }
        public Point RequestedPos { get; set; }
        public GameMoveEntity(PieceType type, Point currentPos, Point requestedPos, Color color)
        {
            CurrentPos = currentPos;
            RequestedPos = requestedPos;
            Color = color;
            Type = type;
        }
        public GameMoveEntity(Point currentPos, Point requestedPos)
        {
            CurrentPos = currentPos;
            RequestedPos = requestedPos;
            Color = Color.None;
            Type = PieceType.None;
        }
    }
}
