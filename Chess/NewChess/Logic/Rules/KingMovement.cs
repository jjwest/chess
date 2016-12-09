using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Logic.Rules
{
    public class KingMovement : Rule
    {
        public bool IsValid(GameMoveEntity movement, GameStateEntity state)
        {
            if (!movement.Type.Equals(PieceType.King))
                return true;

            return CastleMovement(movement, state) || NormalMovement(movement, state);
        }
        private bool CastleMovement(GameMoveEntity movement, GameStateEntity state)
        {
            var king = state.GameBoard.GetPieceAt(movement.CurrentPos);
            var rook = state.GameBoard.GetPieceAt(movement.RequestedPos);
            Point newKingPos = movement.RequestedPos;

            if (movement.RequestedPos.X == 0 && movement.RequestedPos.Y == 0)
                newKingPos = new Point(1, 0);
            else if (movement.RequestedPos.X == 7 && movement.RequestedPos.Y == 0)
                newKingPos = new Point(5, 0);
            else if (movement.RequestedPos.X == 0 && movement.RequestedPos.Y == 7)
                newKingPos = new Point(2, 7);
            else if (movement.RequestedPos.X == 7 && movement.RequestedPos.Y == 7)
                newKingPos = new Point(6, 7);

            GameStateEntity mockState = state.Clone();
            mockState.GameBoard.PlacePieceAt(newKingPos, mockState.GameBoard.GetPieceAt(movement.CurrentPos));
            mockState.GameBoard.PlacePieceAt(movement.CurrentPos, new GamePiece(PieceType.None, Color.None));

            return !king.HasMoved &&
                   !rook.HasMoved &&
                    rook.Type == PieceType.Rook &&
                    Utilities.PathIsClear(movement, state.GameBoard) &&
                    Utilities.StepOnOwnPiece(movement, state) &&
                   !Utilities.CheckedAfterCastling(mockState, mockState.ActivePlayer);
        }
        private bool NormalMovement(GameMoveEntity movement, GameStateEntity state)
        {
            if (Utilities.StepOnOwnPiece(movement, state))
                return false;

            int deltaX = Math.Abs(movement.RequestedPos.X - movement.CurrentPos.X);
            int deltaY = Math.Abs(movement.RequestedPos.Y - movement.CurrentPos.Y);
            return deltaX < 2 && deltaY < 2;
        }
    }
}
