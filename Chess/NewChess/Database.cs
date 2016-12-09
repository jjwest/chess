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

        string DatabasePath { get; set; }
    }

    public class SerializableState
    {
        public bool PawnIsPromoted { get; set; }
        public bool KingIsChecked { get; set; }
        public Color ActivePlayer { get; set; }
        public Color Winner { get; set; }
        public List<GamePiece> GameBoard { get; set; }

        public SerializableState() { }

        public SerializableState(GameStateEntity state)
        {
            GameBoard = new List<GamePiece>();

            for (int y = 0; y < state.GameBoard.Width(); y++)
            {
                for (int x = 0; x < state.GameBoard.Width(); x++)
                {
                   GameBoard.Add(state.GameBoard.GetPieceAt(new Point(x, y)));
                }
            }

            PawnIsPromoted = state.PawnIsPromoted;
            KingIsChecked = state.KingIsChecked;
            ActivePlayer = state.ActivePlayer;
            Winner = state.Winner;
        }
    }

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
                    Board board = new Board(deserialized.GameBoard);
                    GameStateEntity state = new GameStateEntity(board);
                    state.ActivePlayer = deserialized.ActivePlayer;
                    state.PawnIsPromoted = deserialized.PawnIsPromoted;
                    state.KingIsChecked = deserialized.KingIsChecked;
                    state.Winner = deserialized.Winner;

                    return state;
                }
            }
            catch (FileNotFoundException ex)
            {
                return GetDefaultState();
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
            return new GameStateEntity(new Board(
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