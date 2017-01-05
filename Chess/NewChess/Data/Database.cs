using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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
                var database = XDocument.Load(DatabasePath).Element("root");
                var boardNodes = database.Descendants("board").Elements();
                var gamepieces = new List<GamePiece>();

                foreach (var node in boardNodes)
                {
                    Console.WriteLine("INNEHÅLL " + node.Element("type").Value);
                    PieceType type = (PieceType)Enum.Parse(typeof(PieceType), node.Element("type").Value);
                    Color color = (Color)Enum.Parse(typeof(Color), node.Element("color").Value);
                    gamepieces.Add(new GamePiece(type, color));
                }

                var gameBoard = new GameBoard(gamepieces);
                var gameState = new GameStateEntity(gameBoard);

                gameState.ActivePlayer = (Color)Enum.Parse(typeof(Color), database.Element("active_player").Value);
                gameState.PawnIsPromoted = Boolean.Parse(database.Element("pawn_is_promoted").Value);
                gameState.KingIsChecked = Boolean.Parse(database.Element("king_is_checked").Value);
                gameState.Winner = (Color)Enum.Parse(typeof(Color), database.Element("winner").Value);

                return gameState;
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
            try
            {
                XElement board = new XElement("board");
                for (int y = 0; y < _state.GameBoard.Width(); y++)
                {
                    for (int x = 0; x < _state.GameBoard.Width(); x++)
                    {
                        var piece = _state.GameBoard.GetPieceAt(new Point(x, y));
                        board.Add(new XElement("GamePiece",
                                    new XElement("type", piece.Type.ToString()),
                                    new XElement("color", piece.Color.ToString())));
                    }
                }

                XDocument database = new XDocument(
                        new XElement("root",
                            board,
                            new XElement("active_player", _state.ActivePlayer),
                            new XElement("pawn_is_promoted", _state.PawnIsPromoted),
                            new XElement("winner", _state.Winner),
                            new XElement("king_is_checked", _state.KingIsChecked))
                    );

                database.Save(DatabasePath);
            }
            
            catch (Exception)
            {
                throw new DatabaseSaveFailureException("Failed to save to datbase");
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
