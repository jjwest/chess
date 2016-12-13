using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Rules;
using Entities;
using Exceptions;
using Data;

namespace Logic
{
    public class GameLogic : ILogic
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
            try
            {
                return database.GetState();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public GameStateEntity MovePiece(GameMoveEntity movement)
        {
            try
            {
                var gameState = database.GetState();
                movement = SetColorAndType(movement, gameState);

                if (ruleBook.MoveIsValid(movement, gameState))
                {
                    gameState = ExecuteMove(movement, gameState);
                    gameState = CheckForPromotion(movement, gameState);
                    gameState = ChangeActivePlayer(gameState);
                    database.SaveState(gameState);

                    return database.GetState();
                }

                return gameState;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        private GameMoveEntity SetColorAndType(GameMoveEntity movement, GameStateEntity state)
        {
            movement.Color = state.GameBoard.GetPieceAt(movement.CurrentPos).Color;
            movement.Type = state.GameBoard.GetPieceAt(movement.CurrentPos).Type;

            return movement;
        }

        private GameStateEntity ChangeActivePlayer(GameStateEntity state)
        {
            state.ActivePlayer = state.ActivePlayer == Color.White ? Color.Black : Color.White;
            return state;
        }

        private GameStateEntity ExecuteMove(GameMoveEntity movement, GameStateEntity state)
        {
            bool hasMoved = true;
            var movedPiece = state.GameBoard.GetPieceAt(movement.CurrentPos);
            var target = state.GameBoard.GetPieceAt(movement.RequestedPos);
            var opponentColor = state.ActivePlayer == Color.White ? Color.Black : Color.White;

            if (Castling(movedPiece, target))
            {
                state = PerformCastling(movement, state);
            }
            else
            {
                state.GameBoard.PlacePieceAt(movement.CurrentPos, new GamePiece(PieceType.None, Color.None));
                state.GameBoard.PlacePieceAt(movement.RequestedPos, new GamePiece(movement.Type, movement.Color, hasMoved));
            }
            if (ruleBook.KingIsChecked(state, opponentColor))
            {
                if (Checkmate(state))
                {
                    state.Winner = state.ActivePlayer;
                }
                state.KingIsChecked = true;
                return state;
            }

            state.KingIsChecked = false;
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
            List<Point> aggressors = FindPiecesThatReachTarget(state, kingPos, state.ActivePlayer);
            var clonedState = state.Clone();
            clonedState.ActivePlayer = enemyColor;

            return !CanMoveKing(clonedState) && !CanCaptureAggressor(clonedState, aggressors) && !CanBlockPath(clonedState, aggressors);
        }
        private bool CanMoveKing(GameStateEntity state)
        {
            List<GameMoveEntity> positions = new List<GameMoveEntity>();
            var king = Utilities.FindKing(state, state.ActivePlayer);

            for (int y = king.Y - 1; y <= king.Y + 1; y++)
            {
                for (int x = king.X - 1; x <= king.X + 1; x++)
                {
                    if (x >= 0 && y >= 0 && x < 8 && y < 8)
                    {
                        positions.Add(new GameMoveEntity(PieceType.King, king, new Point(x, y), state.ActivePlayer));
                    }
                }
            }

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

        private GameStateEntity CheckForPromotion(GameMoveEntity movement, GameStateEntity state)
        {
            if (movement.Type == PieceType.Pawn)
            {
                if (state.ActivePlayer == Color.White)
                    state.PawnIsPromoted = movement.RequestedPos.Y == 0;
                else
                    state.PawnIsPromoted = movement.RequestedPos.Y == state.GameBoard.Width() - 1;
            }

            return state;
        }

        public GameStateEntity TransformPiece(GameMoveEntity piece)
        {
            try
            {
                var state = database.GetState();
                state.GameBoard.PlacePieceAt(piece.RequestedPos, new GamePiece(piece.Type, piece.Color));
                state.PawnIsPromoted = false;

                // We need to swap active player when checking if king is checked
                // since the rules mandate that you can only move your own piece
                Color opponentColor = state.ActivePlayer;
                state.ActivePlayer = state.ActivePlayer == Color.White ? Color.Black : Color.White;
                state.KingIsChecked = ruleBook.KingIsChecked(state, opponentColor);
                state.ActivePlayer = state.ActivePlayer == Color.White ? Color.Black : Color.White;

                database.SaveState(state);

                return database.GetState();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ResetBoard()
        {
            try
            {
                database.ResetBoard();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}