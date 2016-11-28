using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules;
using Entities;
using Enums;
using Database;
using Utility;

namespace Logic
{
    public class GameLogic
    {
        private DatabaseInterface database;
        private RuleBook ruleBook;
        public GameLogic(DatabaseInterface _database, RuleBook _rules)
        {
            database = _database;
            ruleBook = _rules;
        }

        public GameStateEntity GetInitialState()
        {
            return database.GetState();
        }
        public GameStateEntity MovePiece(GameMoveEntity piece)
        {
            var gameState = database.GetState();
            if (ruleBook.MoveIsValid(piece, gameState))
            {
                gameState = ExecuteMove(piece, gameState);
                database.SaveState(gameState);
                return database.GetState();
            }
            return gameState;
        }
        private GameStateEntity ExecuteMove(GameMoveEntity piece, GameStateEntity state)
        {
            bool hasMoved = true;
            var movedPiece = state.GameBoard.GetPieceAt(piece.CurrentPos);
            var target = state.GameBoard.GetPieceAt(piece.RequestedPos);
            var opponentColor = state.ActivePlayer == Color.White ? Color.Black : Color.White;

            if (Castling(movedPiece, target))
            {
                state = PerformCastling(piece, state);
            }
            else
            {
                state.GameBoard.PlacePieceAt(piece.CurrentPos, new GamePiece(PieceType.None, Color.None));
                state.GameBoard.PlacePieceAt(piece.RequestedPos, new GamePiece(piece.Type, piece.Color, hasMoved));
            }
            if (Utilities.KingIsChecked(state, opponentColor))
            {
                Console.WriteLine("KING IS CHECKED");
                if (Checkmate(state))
                {
                    state.Winner = state.ActivePlayer;
                    Console.WriteLine("GAME OVER!");
                }
                state.KingIsChecked = true;
            }
            state.ActivePlayer = opponentColor;
            return state;
        }
        private bool Castling(GamePiece movedPiece, GamePiece target)
        {
            return movedPiece.Type == PieceType.King &&
                   target.Type == PieceType.Rook &&
                   movedPiece.Color == target.Color;
        }
        private GameStateEntity PerformCastling(GameMoveEntity piece, GameStateEntity state)
        {
            var hasMoved = true;
            if (piece.Color == Color.White)
            {
                if (piece.RequestedPos.X == 7)
                {
                    state.GameBoard.PlacePieceAt(new Point(7, 7), new GamePiece(PieceType.None, Color.None));
                    state.GameBoard.PlacePieceAt(new Point(4, 7), new GamePiece(PieceType.None, Color.None));
                    state.GameBoard.PlacePieceAt(new Point(6, 7), new GamePiece(PieceType.King, Color.White, hasMoved));
                    state.GameBoard.PlacePieceAt(new Point(5, 7), new GamePiece(PieceType.Rook, Color.White, hasMoved));
                }
                else
                {
                    state.GameBoard.PlacePieceAt(new Point(0, 7), new GamePiece(PieceType.None, Color.None));
                    state.GameBoard.PlacePieceAt(new Point(4, 7), new GamePiece(PieceType.None, Color.None));
                    state.GameBoard.PlacePieceAt(new Point(2, 7), new GamePiece(PieceType.King, Color.White, hasMoved));
                    state.GameBoard.PlacePieceAt(new Point(3, 7), new GamePiece(PieceType.Rook, Color.White, hasMoved));
                }
            }
            else
            {
                if (piece.RequestedPos.X == 7)
                {
                    state.GameBoard.PlacePieceAt(new Point(7, 0), new GamePiece(PieceType.None, Color.None));
                    state.GameBoard.PlacePieceAt(new Point(3, 0), new GamePiece(PieceType.None, Color.None));
                    state.GameBoard.PlacePieceAt(new Point(5, 0), new GamePiece(PieceType.King, Color.Black, hasMoved));
                    state.GameBoard.PlacePieceAt(new Point(4, 0), new GamePiece(PieceType.Rook, Color.Black, hasMoved));
                }
                else
                {
                    state.GameBoard.PlacePieceAt(new Point(0, 0), new GamePiece(PieceType.None, Color.None));
                    state.GameBoard.PlacePieceAt(new Point(3, 0), new GamePiece(PieceType.None, Color.None));
                    state.GameBoard.PlacePieceAt(new Point(1, 0), new GamePiece(PieceType.King, Color.Black, hasMoved));
                    state.GameBoard.PlacePieceAt(new Point(2, 0), new GamePiece(PieceType.Rook, Color.Black, hasMoved));
                }
            }
            return state;
        }
        private bool Checkmate(GameStateEntity state)
        {
            Color enemyColor = state.ActivePlayer == Color.White ? Color.Black : Color.White;
            Point kingPos = Utilities.FindKing(state, enemyColor);
            List<Point> aggressors = FindPiecesThatReachTarget(state, kingPos, enemyColor);
            return !CanMoveKing(state) && !CanCaptureAggressor(state, aggressors) && !CanBlockPath(state, aggressors);
        }
        private bool CanMoveKing(GameStateEntity state)
        {
            List<GameMoveEntity> positions = new List<GameMoveEntity>();
            Color kingColor = state.ActivePlayer == Color.White ? Color.Black : Color.White;
            var king = Utilities.FindKing(state, kingColor);
            for (int y = king.Y - 1; y <= king.Y + 1; y++)
                for (int x = king.X - 1; y <= king.X + 1; x++)
                    if (x >= 0 && y >= 0 && x < 8 && y < 8)
                        positions.Add(new GameMoveEntity(PieceType.King, king, new Point(x, y), kingColor));
            return positions.Any(pos => ruleBook.MoveIsValid(pos, state));
        }
        private List<Point> FindPiecesThatReachTarget(GameStateEntity state, Point target, Color playerColor)
        {
            var piecesThatReachTarget = new List<Point>();
            var board = state.GameBoard;
            for (int y = 0; y < board.Width(); y++)
            {
                for (int x = 0; x < board.Width(); x++)
                {
                    var piece = board.GetPieceAt(new Point(x, y));
                    if (piece.Color == playerColor)
                    {
                        GameMoveEntity move = new GameMoveEntity(piece.Type, new Point(x, y), target, piece.Color);
                        if (ruleBook.MoveIsValid(move, state))
                        {
                            piecesThatReachTarget.Add(new Point(x, y));
                        }
                    }
                }
            }
            return piecesThatReachTarget;
        }
        private bool CanCaptureAggressor(GameStateEntity state, List<Point> aggressors)
        {
            if (aggressors.Count() > 1)
                return false;
            Point aggressor = aggressors[0];
            if (FindPiecesThatReachTarget(state, aggressor, state.ActivePlayer).Count() > 0)
                return true;
            return false;
        }
        private bool CanBlockPath(GameStateEntity state, List<Point> aggressors)
        {
            if (aggressors.Count() > 1)
                return false;
            var aggressor = state.GameBoard.GetPieceAt(aggressors[0]);
            if (aggressor.Type == PieceType.Bishop ||
                aggressor.Type == PieceType.Rook ||
                aggressor.Type == PieceType.Queen)
            {
                Color kingColor = state.ActivePlayer == Color.White ? Color.Black : Color.White;
                var kingPos = Utilities.FindKing(state, kingColor);
                var blockPositions = FindBlockPositions(state, kingPos, aggressors[0]);
                return blockPositions.Any(pos => FindPiecesThatReachTarget(state, pos, kingColor).Count() > 0);
            }
            return false;
        }
        private List<Point> FindBlockPositions(GameStateEntity state, Point king, Point aggressor)
        {
            int deltaX = king.X - aggressor.X;
            int deltaY = king.Y - aggressor.Y;
            int stepX = deltaX == 0 ? 0 : deltaX / System.Math.Abs(deltaX);
            int stepY = deltaY == 0 ? 0 : deltaY / System.Math.Abs(deltaY);
            var board = state.GameBoard;
            var blockPositions = new List<Point>();
            for (int i = 1; i < Math.Max(Math.Abs(deltaX), Math.Abs(deltaY)); i++)
            {
                Point position = new Point(king.X + i * stepX, king.Y + i * stepY);
                blockPositions.Add(position);
            }
            return blockPositions;
        }
        public GameStateEntity TransformPiece(GameMoveEntity piece)
        {
            var gameState = database.GetState();
            gameState.GameBoard.PlacePieceAt(piece.RequestedPos, new GamePiece(piece.Type, piece.Color));
            database.SaveState(gameState);
            return gameState;
        }
    }
}