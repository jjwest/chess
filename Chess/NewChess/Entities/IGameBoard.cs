using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public interface IGameBoard : ICloneable
    {
        int Width();

        GamePiece GetPieceAt(Point p);

        void PlacePieceAt(Point p, GamePiece piece);
    }
}
