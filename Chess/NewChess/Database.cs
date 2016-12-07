using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Entities;

namespace Database
{
    public interface DatabaseInterface
    {
        GameStateEntity GetState();
        void ResetBoard();
        void SaveState(GameStateEntity gameState);
    }
    public class Database : DatabaseInterface
    {
        private GameStateEntity state = new GameStateEntity( new Board(
            new List<GamePiece> {
                new GamePiece(PieceType.King, Color.Black),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.King, Color.White),
                new GamePiece(PieceType.Rook, Color.White),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),

                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),

                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.Pawn, Color.White),
                new GamePiece(PieceType.Pawn, Color.White),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),

                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),

                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),

                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),

                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),

                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None),
                new GamePiece(PieceType.None, Color.None)
        }));
        public GameStateEntity GetState()
        {
            return state;
        }
        public void ResetBoard()
        {
        }
        public void SaveState(GameStateEntity _state)
        {
            state = _state;
            MemoryStream memstream = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GameStateEntity));
            ser.WriteObject(memstream, _state);
            FileStream fs = File.Create("database.json");
            memstream.WriteTo(fs);
            memstream.Close();
            fs.Close();
      
        }
    }
}