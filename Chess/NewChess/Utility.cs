using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entities;
using Enums;

namespace Utility
{
    public class Utilities
    {
        public static bool StepOnOwnPiece(GameMoveEntity movement, GameStateEntity state)
        {
            return state.GameBoard.GetPieceAt(movement.RequestedPos).Color == movement.Color;
        }
        public static bool IsLinear(GameMoveEntity movement, GameStateEntity state)
        {
            return (movement.RequestedPos.X == movement.CurrentPos.X ||
                    movement.RequestedPos.Y == movement.CurrentPos.Y);
        }
        public static bool IsDiagonal(GameMoveEntity movement, GameStateEntity state)
        {
            int deltaX = Math.Abs(movement.RequestedPos.X - movement.CurrentPos.X);
            int deltaY = Math.Abs(movement.RequestedPos.Y - movement.CurrentPos.Y);
            return deltaX == deltaY;
        }
        public static bool PathIsClear(GameMoveEntity movement, GameBoard board)
        {
            int deltaX = movement.RequestedPos.X - movement.CurrentPos.X;
            int deltaY = movement.RequestedPos.Y - movement.CurrentPos.Y;
            int stepX = deltaX == 0 ? 0 : deltaX / System.Math.Abs(deltaX);
            int stepY = deltaY == 0 ? 0 : deltaY / System.Math.Abs(deltaY);
            int currX = movement.CurrentPos.X;
            int currY = movement.CurrentPos.Y;
            for (int i = 1; i < Math.Max(Math.Abs(deltaX), Math.Abs(deltaY)); i++)
            {
                var piece = board.GetPieceAt(new Point(currX + i * stepX, currY + i * stepY));
                if (piece.Type != PieceType.None)
                    return false;
            }
            return true;
        }
        public static bool KingIsChecked(GameStateEntity state, Color kingColor)
        {
            var board = state.GameBoard;
            Point king = FindKing(state, kingColor);
            Console.WriteLine(String.Format("KING: {0}, {1}", king.X, king.Y));
            for (int y = 0; y < board.Width(); y++)
            {
                for (int x = 0; x < board.Width(); x++)
                {
                    //Det saknas ett IF statement som gör att funktionen kan returnera true.
                    var piece = state.GameBoard.GetPieceAt(new Point(x, y));
                    
                }
            }
            return false;
        }

        public static GamePiece[][] DeepCopy(GamePiece[][] original)
        {
            GamePiece[][] newBoard = new GamePiece[original.Length][];
            for (int i = 0; i < newBoard.Length; i++)
            {
                newBoard[i] = (GamePiece[])original[i].Clone();
            }
            return newBoard;
        }
        public static Point FindKing(GameStateEntity state, Color kingColor)
        {
            var board = state.GameBoard;
            Point king = new Point(0, 0);
            for (int y = 0; y < state.GameBoard.Width(); y++)
            {
                for (int x = 0; x < state.GameBoard.Width(); x++)
                {
                    var type = state.GameBoard.GetPieceAt(new Point(x, y)).Type;
                    var color = state.GameBoard.GetPieceAt(new Point(x, y)).Color;
                    if (type == PieceType.King && color == kingColor)
                    {
                        king = new Point(x, y);
                        break;
                    }
                }
            }
            return king;
        }
    }
}