using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exceptions;

namespace Entities
{
    public class GameBoard : IGameBoard, ICloneable
    {
        protected const int width = 8;
        protected List<GamePiece> gameBoard;

        private GameBoard() { }

        public GameBoard(List<GamePiece> board)
        {
            gameBoard = board;
        }

        public int Width()
        {
            return width;
        }

        public Object Clone()
        {
            var clone = new List<GamePiece>();
            foreach (var piece in gameBoard)
            {
                var clonedPiece = new GamePiece(PieceType.None, Color.None);
                clonedPiece.Type = piece.Type;
                clonedPiece.Color = piece.Color;
                clone.Add(clonedPiece);
            }

            return new GameBoard(clone);
        }

        public GamePiece GetPieceAt(Point p)
        {
            try
            {
                return gameBoard[ GetIndex(p) ];
            }
            catch (Exception)
            {
                throw new InvalidBoardPositionException();
            }      
        }

        public void PlacePieceAt(Point p, GamePiece piece)
        {
            try
            {
                gameBoard [GetIndex(p) ] = piece;
            }
            catch (Exception)
            {
                throw new InvalidBoardPositionException();
            }
        }

        private int GetIndex(Point p)
        {
            return p.Y * width + p.X;
        }
    }
}
