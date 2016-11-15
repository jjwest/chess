using System;
using Entities;
using Enums;

namespace Utility
{
    public class Utilities
    {
        public static bool StepOnOwnPiece(GameMoveEntity piece, GameStateEntity state)
        {
            return state.GameBoard[piece.RequestedPos.Y] [piece.RequestedPos.X].Color == piece.Color;
        }

        public static bool IsLinear(GameMoveEntity piece, GameStateEntity state)
        {
            return (piece.RequestedPos.X == piece.CurrentPos.X ||
                    piece.RequestedPos.Y == piece.CurrentPos.Y);
        }

        public static bool IsDiagonal(GameMoveEntity piece, GameStateEntity state)
        {
            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = Math.Abs(piece.RequestedPos.Y - piece.CurrentPos.Y);

            return deltaX == deltaY;
        }

        public static bool PathIsClear(GameMoveEntity piece, GamePiece[][] board)
        {
            int deltaX = piece.RequestedPos.X - piece.CurrentPos.X;
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;
            int stepX = deltaX == 0 ? 0 : deltaX / System.Math.Abs(deltaX);
            int stepY = deltaY == 0 ? 0 : deltaY / System.Math.Abs(deltaY);
            int currX = piece.CurrentPos.X;
            int currY = piece.CurrentPos.Y;

            for (int i = 1; i < Math.Max(Math.Abs(deltaX), Math.Abs(deltaY)); i++)
            {
                if (board[currY + i * stepY][currX + i * stepX].Type != PieceType.None)
                    return false;
            }

            return true;
        }


        public static bool KingIsChecked(GameStateEntity state, Color kingColor)
        {
            var board = state.GameBoard;

            Point king = FindKing(state, kingColor);
            Console.WriteLine(String.Format("KING: {0}, {1}", king.X, king.Y));
           
            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    var type = board[y][x].Type;
                    var color = board[y][x].Color;

                }
            }

            return false;
        }

//        public static bool KingIsChecked(GameStateEntity state)
//        {
//            var board = state.GameBoard;
//
//            Point king = FindKing(state, state.ActivePlayer);
//            Console.WriteLine(String.Format("KING: {0}, {1}", king.X, king.Y));
//           
//            for (int y = 0; y < board.Length; y++)
//            {
//                for (int x = 0; x < board[y].Length; x++)
//                {
//                    var type = board[y][x].Type;
//                    var color = board[y][x].Color;
//                    if ((type == PieceType.Bishop || type == PieceType.Queen) && color != state.ActivePlayer)
//                    {
//                        var potentialThreat = new GameMoveEntity(type, new Point(x, y), new Point(king.X, king.Y), state.ActivePlayer);
//                        if (Utility.IsDiagonal(potentialThreat, state) && Utility.PathIsClear(potentialThreat, board))
//                            return true;
//                    }
//                    if ((type == PieceType.Rook || type == PieceType.Queen) && color != state.ActivePlayer)
//                    {
//                        Console.WriteLine("INNE I ROOK");
//                        var potentialThreat = new GameMoveEntity(type, new Point(x, y), new Point(king.X, king.Y), state.ActivePlayer);
//                        if (Utility.IsLinear(potentialThreat, state) && Utility.PathIsClear(potentialThreat, board))
//                            return true;
//                    }
//                }
//            }
//
//            return false;
//        }

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

            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    var type = board[y][x].Type;
                    var color = board[y][x].Color;
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

