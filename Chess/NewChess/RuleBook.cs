using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rules;
using Entities;
using Utility;

namespace Rules
{
    public class RuleBook
    {
        private List<Rule> rules = new List<Rule>();
        public void AddRule(Rule rule)
        {
            rules.Add(rule);
        }
        public bool MoveIsValid(GameMoveEntity movement, GameStateEntity state)
        {
            bool moveIsAllowed = rules.All(rule => rule.IsValid(movement, state));
            bool kingIsChecked = false;

            if (moveIsAllowed)
            {
                var clonedState = state.Clone();

                clonedState.GameBoard.PlacePieceAt(movement.RequestedPos, new GamePiece(movement.Type, movement.Color));
                clonedState.GameBoard.PlacePieceAt(movement.CurrentPos, new GamePiece(PieceType.None, Color.None));
                clonedState.ActivePlayer = state.ActivePlayer == Color.White ? Color.Black : Color.White;
                kingIsChecked = KingIsChecked(clonedState, state.ActivePlayer);
            }

            return moveIsAllowed && !kingIsChecked;
        }

        public bool KingIsChecked(GameStateEntity state, Color kingColor)
        {
            Console.WriteLine("Active player: " + state.ActivePlayer);
            Console.WriteLine("King color: " + kingColor);
            var board = state.GameBoard;
            var kingPos = Utilities.FindKing(state, kingColor);
            var opponentColor = kingColor == Color.White ? Color.Black : Color.White;
         
            for (int y = 0; y < board.Width(); y++)
            {
                for (int x = 0; x < board.Width(); x++)
                {
                    var piece = board.GetPieceAt(new Point(x, y));
       
                    if (piece.Color == opponentColor)
                    {
                        GameMoveEntity moveToKing = new GameMoveEntity(piece.Type, new Point(x, y), kingPos, piece.Color);

                        if (rules.All(rule => rule.IsValid(moveToKing, state)))
                        {
                            return true;
                        }
                           
                    }
                }
            }

            return false;
        }
    }
}