using System;
using System.Linq;
using System.Collections.Generic;
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

    	public bool MoveIsValid(GamePieceEntity piece, GameStateEntity gameBoard)
    	{
    	    return rules.All(rule => rule.IsValid(piece, gameBoard));
    	}
    }
}
