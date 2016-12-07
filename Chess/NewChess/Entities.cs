﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entities
{
    public enum PieceType
    {
        Pawn,
        Bishop,
        Knight,
        Rook,
        Queen,
        King,
        None
    }
    public enum Color
    {
        White,
        Black,
        None
    }
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
        public bool PawnIsPromoted { get; set; } = false;
        public bool KingIsChecked { get; set; } = false;
        public Color ActivePlayer { get; set; } = Color.White;
        public Color Winner { get; set; } = Color.None;
        public GameBoard GameBoard { get; set; }

        public GameStateEntity(GameBoard gameBoard)
        {
            GameBoard = gameBoard;
        }
        public GameStateEntity Clone()
        {
            var deepCopy = (GameStateEntity)this.MemberwiseClone();
            deepCopy.GameBoard = (GameBoard)deepCopy.GameBoard.Clone();

            return deepCopy;
        }

    }
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

    public interface GameBoard : ICloneable
    {
        int Width();

        GamePiece GetPieceAt(Point p);

        void PlacePieceAt(Point p, GamePiece piece);
    }

    public class Board : GameBoard, ICloneable
    {
        protected const int width = 8;
        protected List<GamePiece> gameBoard;


        private Board() {}

        public Board(List<GamePiece> board)
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

            return new Board(clone);
        }

        public GamePiece GetPieceAt(Point p)
        {
            return gameBoard[ GetIndex(p) ];
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