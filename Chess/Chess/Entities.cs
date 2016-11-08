using System;
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
    	public bool PawnIsPromoted { get; set; } = false;
    	public bool KingIsChecked { get; set; } = false;
    	public Player ActivePlayer { get; set; } = Player.White;
    	public Player Winner { get; set; } = Player.None;
        public GamePiece [][] GameBoard { get; set; }

        public GameStateEntity(GamePiece [][] gameBoard)
    	{
    	    GameBoard = gameBoard;
    	}
    }

    public class GamePiece
    {
        public PieceType Type { get; set; }
        public Player Color { get; set; }
        public bool HasMoved { get; set; } 

        public GamePiece(PieceType piece, Player color)
        {
            Type = piece;
            Color = color;
            HasMoved = false;
        }

        public GamePiece(PieceType piece, Player color, bool hasMoved)
        {
            Type = piece;
            Color = color;
            HasMoved = hasMoved;
        }
    }

    public class GamePieceEntity
    {
    	public Player Color { get; set; }
        public PieceType Type { get; set; }
    	public Point CurrentPos { get; set; }
    	public Point RequestedPos { get; set; }

        public GamePieceEntity(PieceType type, Point currentPos, Point requestedPos, Player color)
    	{
    	    Type = type;
    	    CurrentPos = currentPos;
    	    RequestedPos = requestedPos;
            Color = color;
    	}
    }
}
