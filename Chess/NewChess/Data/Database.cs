using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Entities;
using Data;
using Exceptions;


namespace Data
{
    public class Database : DatabaseInterface
    {
        public string DatabasePath { get; set; } = "database.xml";

        public GameStateEntity GetState()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(SerializableState));
                using (FileStream fs = File.OpenRead(DatabasePath))
                {
                    var deserialized = (SerializableState)serializer.Deserialize(fs);
                    GameBoard board = new GameBoard(deserialized.GameBoard);
                    GameStateEntity state = new GameStateEntity(board);
                    state.ActivePlayer = deserialized.ActivePlayer;
                    state.PawnIsPromoted = deserialized.PawnIsPromoted;
                    state.KingIsChecked = deserialized.KingIsChecked;
                    state.Winner = deserialized.Winner;

                    return state;
                }
            }
            catch (FileNotFoundException)
            {
                return GetDefaultState();
            }
            catch (Exception)
            {
                throw new CorruptDatabaseException("Database has been corrupted. Please delete it.");
            }
        }

        public void ResetBoard()
        {
            SaveState(GetDefaultState());
        }

        public void SaveState(GameStateEntity _state)
        {
            var serializableState = new SerializableState(_state);
            var serializer = new XmlSerializer(typeof(SerializableState));

            using (FileStream fs = File.Create(DatabasePath))
            {
                serializer.Serialize(fs, serializableState);
            }
        }

        private GameStateEntity GetDefaultState()
        {
            return new GameStateEntity(new GameBoard(
              new List<GamePiece> {
                new GamePiece(PieceType.Rook, Color.Black),
                new GamePiece(PieceType.Knight, Color.Black),
                new GamePiece(PieceType.Bishop, Color.Black),
                new GamePiece(PieceType.Queen, Color.Black),
                new GamePiece(PieceType.King, Color.Black),
                new GamePiece(PieceType.Bishop, Color.Black),
                new GamePiece(PieceType.Knight, Color.Black),
                new GamePiece(PieceType.Rook, Color.Black),

                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.Pawn, Color.Black),
                new GamePiece(PieceType.Pawn, Color.Black),

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

                new GamePiece(PieceType.Pawn, Color.White),
                new GamePiece(PieceType.Pawn, Color.White),
                new GamePiece(PieceType.Pawn, Color.White),
                new GamePiece(PieceType.Pawn, Color.White),
                new GamePiece(PieceType.Pawn, Color.White),
                new GamePiece(PieceType.Pawn, Color.White),
                new GamePiece(PieceType.Pawn, Color.White),
                new GamePiece(PieceType.Pawn, Color.White),

                new GamePiece(PieceType.Rook, Color.White),
                new GamePiece(PieceType.Knight, Color.White),
                new GamePiece(PieceType.Bishop, Color.White),
                new GamePiece(PieceType.Queen, Color.White),
                new GamePiece(PieceType.King, Color.White),
                new GamePiece(PieceType.Bishop, Color.White),
                new GamePiece(PieceType.Knight, Color.White),
                new GamePiece(PieceType.Rook, Color.White)
            }));
        }
    }
}
