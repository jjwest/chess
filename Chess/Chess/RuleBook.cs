using System;
using System.Linq;
using System.Collections.Generic;
using Rules;
using Entities;

namespace Rules
{
	public class RuleBook
	{
		private  List<Rule> rules;

		public RuleBook (List<Rule> _rules)
		{
			rules = _rules;
		}

		public bool MoveIsValid(GamePieceEntity piece, GameStateEntity gameBoard)
		{
			return rules.All(rule => rule.IsValid(piece, gameBoard));
		}
	}
}

