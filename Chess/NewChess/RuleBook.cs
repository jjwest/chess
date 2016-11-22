using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rules;
using Entities;

namespace Rules
{
    public class RuleBook
    {
        private List<Rule> rules = new List<Rule>();
        public void AddRule(Rule rule)
        {
            rules.Add(rule);
        }
        public bool MoveIsValid(GameMoveEntity piece, GameStateEntity gameBoard)
        {
            return rules.All(rule => rule.IsValid(piece, gameBoard));
        }
    }
}