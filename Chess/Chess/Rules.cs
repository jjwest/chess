using System;
using System.Linq;
using Enums;
using Entities;


namespace Rules
{
    public interface Rule
    {
        bool IsValid(GameMoveEntity piece, GameStateEntity gameBoard);
    }

    public class Movement
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

    	public static bool PathIsClear(GameMoveEntity piece, GameStateEntity state)
    	{
    	    int deltaX = piece.RequestedPos.X - piece.CurrentPos.X;
    	    int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;
    	    int stepX = deltaX == 0 ? 0 : deltaX / System.Math.Abs(deltaX);
    	    int stepY = deltaY == 0 ? 0 : deltaY / System.Math.Abs(deltaY);
            int currX = piece.CurrentPos.X;
            int currY = piece.CurrentPos.Y;

            for (int i = 1; i < Math.Max(Math.Abs(deltaX), Math.Abs(deltaY)); i++)
	    {
                if (state.GameBoard[currY + i * stepY][currX + i * stepX].Type != PieceType.None)
                    return false;
    	    }

            return true;
    	}

        public static bool KingIsChecked(GameMoveEntity piece, GameStateEntity state)
        {
            var board = Movement.DeepCopy(state.GameBoard);
            board[piece.RequestedPos.Y][piece.RequestedPos.X] = board[piece.CurrentPos.Y][piece.CurrentPos.X];
            board[piece.CurrentPos.Y][piece.CurrentPos.X] = new GamePiece(PieceType.None, Color.None);
            Point king = FindKing(piece, state);

            for (int y = 0; y < state.GameBoard.Length; y++)
            {
                for (int x = 0; x < state.GameBoard[y].Length; x++)
                {
                    var type = state.GameBoard[y][x].Type;
                    var color = state.GameBoard[y][x].Color;
                    if ((type == PieceType.Bishop || type == PieceType.Queen) && color != piece.Color)
                    {
                        var potentialThreat = new GameMoveEntity(type, new Point(x, y), new Point(king.X, king.Y), piece.Color);
                        if (Movement.IsDiagonal(potentialThreat, state) && Movement.PathIsClear(potentialThreat, state))
                            return false;
                    }
                    if ((type == PieceType.Rook || type == PieceType.Queen) && color != piece.Color)
                    {
                        var potentialThreat = new GameMoveEntity(type, new Point(x, y), new Point(king.X, king.Y), piece.Color);
                        if (Movement.IsLinear(potentialThreat, state) && Movement.PathIsClear(potentialThreat, state))
                            return false;
                    }
                }
            }

            Console.WriteLine("NOT CHECKED");
        //          state.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X] = liftedPiece;
            return true;
        }

        private static GamePiece[][] DeepCopy(GamePiece[][] original)
        {
            GamePiece[][] newBoard = new GamePiece[original.Length][];
            for (int i = 0; i < newBoard.Length; i++)
            {
                newBoard[i] = (GamePiece[])original[i].Clone();
            }

            return newBoard;
        }

        private static Point FindKing(GameMoveEntity piece, GameStateEntity state)
        {
            Point king = new Point(0, 0);

            for (int y = 0; y < state.GameBoard.Length; y++)
            {
                for (int x = 0; x < state.GameBoard[y].Length; x++)
                {
                    var type = state.GameBoard[y][x].Type;
                    var color = state.GameBoard[y][x].Color;
                    if (type == PieceType.King && color == piece.Color)
                    {
                        king = new Point(x, y);
                        break;
                    }
                }
            }

            return king;
      }

    }

    public class OnlyMoveOwnPiece : Rule
    {
        public bool IsValid(GameMoveEntity piece, GameStateEntity state)
        {
            return piece.Color == state.ActivePlayer;
        }
    }

    public class RookMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
            if (!piece.Type.Equals(PieceType.Rook))
                return true;
	    if (Movement.StepOnOwnPiece(piece, state))
                return false;

            return Movement.IsLinear(piece, state) && Movement.PathIsClear(piece, state);
	}
    }

    public class PawnMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
            if (!piece.Type.Equals(PieceType.Pawn))
                return true;
            if (Movement.StepOnOwnPiece(piece, state))
                return false;

            return NormalMovement(piece, state) ||
		ChargeMovement(piece, state) ||
		AttackMovement(piece, state);
	}

        private bool NormalMovement(GameMoveEntity piece, GameStateEntity state)
        {
            int deltaX = piece.RequestedPos.X - piece.CurrentPos.X;
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;

            if (deltaX != 0)
                return false;
            if (piece.Color == Color.White)
                return deltaY == -1;
            else
                return deltaY == 1;

        }

        private bool ChargeMovement(GameMoveEntity piece, GameStateEntity state)
        {
            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;
            var gamePiece = state.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X];

            if (piece.Color == Color.White)
                return deltaX == 0 && deltaY == -2 && !gamePiece.HasMoved;
            else
                return deltaX == 0 && deltaY == 2 && !gamePiece.HasMoved;
        }

        private bool AttackMovement(GameMoveEntity piece, GameStateEntity state)
        {
            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = piece.RequestedPos.Y - piece.CurrentPos.Y;
            var target = state.GameBoard[piece.RequestedPos.Y][piece.RequestedPos.X];

            if (piece.Color == Color.White)
                return deltaX == 1 && deltaY == -1 && target.Color == Color.Black;
            else
                return deltaX == 1 && deltaY == 1 && target.Color == Color.Black;
        }
    }

    public class BishopMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
	    if (!piece.Type.Equals(PieceType.Bishop))
		return true;
	    if (Movement.StepOnOwnPiece(piece, state))
                return false;

            return Movement.IsDiagonal(piece, state) && Movement.PathIsClear(piece, state);
	}
    }

    public class KnightMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
            if (!piece.Type.Equals(PieceType.Knight))
                return true;
            if (Movement.StepOnOwnPiece(piece, state))
                return false;

            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = Math.Abs(piece.RequestedPos.Y - piece.CurrentPos.Y);

            return deltaX == 2 && deltaY == 1 || deltaX == 1 && deltaY == 2;
	}
    }

    public class QueenMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
            if (!piece.Type.Equals(PieceType.Queen))
                return true;
            if (Movement.StepOnOwnPiece(piece, state))
                return false;

            return ((Movement.IsLinear(piece, state) || Movement.IsDiagonal(piece, state)) &&
		    Movement.PathIsClear(piece, state));
	}
    }


    public class KingMovement : Rule
    {
	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
	{
            if (!piece.Type.Equals(PieceType.King))
                return true;

            return CastleMovement(piece, state) || NormalMovement(piece, state);
	}

        private bool CastleMovement(GameMoveEntity piece, GameStateEntity state)
        {
            var king = state.GameBoard[piece.CurrentPos.Y][piece.CurrentPos.X];
            var rook = state.GameBoard[piece.RequestedPos.Y][piece.RequestedPos.X];
            Point newKingPos = piece.RequestedPos;

            if (piece.RequestedPos.X == 0 && piece.RequestedPos.Y == 0)
                newKingPos = new Point(1, 0);
            else if (piece.RequestedPos.X == 7 && piece.RequestedPos.Y == 0)
                newKingPos = new Point(5, 0);
            else if (piece.RequestedPos.X == 0 && piece.RequestedPos.Y == 7)
                newKingPos = new Point(2, 7);
            else if (piece.RequestedPos.X == 7 && piece.RequestedPos.Y == 7)
                newKingPos = new Point(6, 7);

            GameMoveEntity mockMove = new GameMoveEntity(PieceType.King, piece.CurrentPos, newKingPos, piece.Color);


            return !king.HasMoved &&
	           	   !rook.HasMoved &&
		            rook.Type == PieceType.Rook &&
		            Movement.PathIsClear(piece, state) &&
		            Movement.StepOnOwnPiece(piece, state) &&
                   !Movement.KingIsChecked(mockMove, state);
        }

        private bool NormalMovement(GameMoveEntity piece, GameStateEntity state)
        {
            if (Movement.StepOnOwnPiece(piece, state))
                return false;

            int deltaX = Math.Abs(piece.RequestedPos.X - piece.CurrentPos.X);
            int deltaY = Math.Abs(piece.RequestedPos.Y - piece.CurrentPos.Y);

            return deltaX < 2 && deltaY < 2;
        }
    }

    public class Check : Rule
    {
    	public bool IsValid(GameMoveEntity piece, GameStateEntity state)
    	{
            return !Movement.KingIsChecked(piece, state);
    	}
    }
}
