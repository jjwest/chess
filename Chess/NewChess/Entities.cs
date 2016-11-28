using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enums;

namespace Entities
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public class GameStateEntity
    {
        public GameStateEntity Clone()
        {
            return (GameStateEntity)this.MemberwiseClone();
        }
        public bool PawnIsPromoted { get; set; } = false;
        public bool KingIsChecked { get; set; } = false;
        public Color ActivePlayer { get; set; } = Color.White;
        public Color Winner { get; set; } = Color.None;
        public GameBoard GameBoard { get; set; }
        public GameStateEntity(GameBoard gameBoard)
        {
            GameBoard = gameBoard;
        }
    }
    public class GamePiece
    {
        public PieceType Type { get; set; }
        public Color Color { get; set; }
        public bool HasMoved { get; set; }
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
    public class GameMoveEntity
    {
        public Color Color { get; set; }
        public PieceType Type { get; set; }
        public Point CurrentPos { get; set; }
        public Point RequestedPos { get; set; }
        public GameMoveEntity(PieceType type, Point currentPos, Point requestedPos, Color color)
        {
            Type = type;
            CurrentPos = currentPos;
            RequestedPos = requestedPos;
            Color = color;
        }
    }

    public interface GameBoard
    {
        int Width();
        GamePiece GetPieceAt(Point p);

        List<GamePiece> GetGameBoardAsList();
        void PlacePieceAt(Point p, GamePiece piece);
    }

    public class Board : GameBoard
    {
        const int width = 8;
        private List<GamePiece> gameBoard;

        public Board(List<GamePiece> board)
        {
            gameBoard = board;
        }

        public int Width()
        {
            return width;
        }

        public GamePiece GetPieceAt(Point p)
        {
            return gameBoard[ GetIndex(p) ];
        }

        public List<GamePiece> GetGameBoardAsList()
        {
            return gameBoard;
        }

        public void PlacePieceAt(Point p, GamePiece piece)
        {
            gameBoard[ GetIndex(p) ] = piece;
        }

        private int GetIndex(Point p)
        {
            return p.Y * width + p.X;
        }
    }
}