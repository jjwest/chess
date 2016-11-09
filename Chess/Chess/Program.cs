using System;
using Gtk;
using Enums;
using System.Linq;
using System.Collections.Generic;
using Rules;
using Entities;

namespace Chess
{
    class MainClass
    {
    	public static void Main (string[] args)
    	{
            var game = new GameLogic(new Database.Database(), LoadRules());           
            var move1 = new GameMoveEntity(PieceType.King, new Point(3, 0), new Point(7, 0), Color.Black);
            var newState = game.MovePiece(move1);
           
            foreach (var row in newState.GameBoard)
            {
                foreach (var column in row)
                {
                    Console.Write(column.Type + " ");
                }
                Console.WriteLine("");
            }
    	}

    	private static RuleBook LoadRules()
    	{
            RuleBook standard = new RuleBook();
            standard.AddRule(new OnlyMoveOwnPiece());
            standard.AddRule(new BishopMovement());
            standard.AddRule(new RookMovement());
            standard.AddRule(new KnightMovement());
            standard.AddRule(new KingMovement());
            standard.AddRule(new QueenMovement());
            standard.AddRule(new PawnMovement());

            return standard;
    	}
    }
}
