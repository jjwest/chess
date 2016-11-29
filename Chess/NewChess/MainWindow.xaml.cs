using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

using Logic;
using Database;
using Rules;
using Entities;

namespace NewChess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Textures textures;
        public MainWindow()
        {
            InitializeComponent();
            textures = new Textures();
            LoadTexture(PieceType.Bishop, "blackBishop.png", "whiteBishop.png");
            LoadTexture(PieceType.Pawn, "blackPawn.png", "whitePawn.png");
            LoadTexture(PieceType.King, "blackKing.png", "whiteKing.png");
            LoadTexture(PieceType.Knight, "blackKnight.png", "whiteKnight.png");
            LoadTexture(PieceType.Queen, "blackQueen.png", "whiteQueen.png");
            LoadTexture(PieceType.Rook, "blackRook.png", "whiteRook.png");

            RuleBook rules = LoadRules();
            GameLogic logic = new GameLogic(new Database.Database(), rules);
            var state = logic.GetInitialState();
            DrawGameBoard(state);
           
          
            // this.GameBoard.Children.Add(img);
            // Grid.SetColumn(img, 3);
            // Grid.SetRow(img, 2);
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
            standard.AddRule(new Check());

            return standard;
        }
        private void DrawGameBoard(GameStateEntity state)
        {
            this.GameBoard.Children.Clear();

            var converter = new BrushConverter();
            var blackBrush = (Brush)converter.ConvertFromString("#FF480000");
            var whiteBrush = (Brush)converter.ConvertFromString("#FFF9D093");

            // Draw board
            for (int y = 0; y < state.GameBoard.Width(); y++)
            {
                for (int x = 0; x < state.GameBoard.Width(); x++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Height = 60;
                    rect.Width = 60;

                    if ((x + y) % 2 == 0)
                        rect.Fill = blackBrush;
                    else
                        rect.Fill = whiteBrush;

                    this.GameBoard.Children.Add(rect);
                    Grid.SetColumn(rect, x);
                    Grid.SetRow(rect, y);
                }
            }



            // Draw pieces
            textures.AddTexturesToGrid(this.GameBoard);

            for (int y = 0; y < state.GameBoard.Width(); y++)
            {
                for (int x = 0; x < state.GameBoard.Width(); x++)
                {
                    var piece = state.GameBoard.GetPieceAt(new Entities.Point(x, y));
                    if (piece.Type != PieceType.None)
                    {
                        var texture = textures.GetTexture(piece);
                        Grid.SetColumn(texture, x);
                        Grid.SetRow(texture, y);
                    }
                }
            }
        }

        public void LoadTexture(PieceType type, string pathToBlack, string pathToWhite)
        {
            BitmapImage blackBitmap = new BitmapImage();
            blackBitmap.BeginInit();
            blackBitmap.UriSource = new Uri(@pathToBlack, UriKind.RelativeOrAbsolute);
            blackBitmap.EndInit();
            Image blackImg = new Image();
            blackImg.Source = blackBitmap;

            BitmapImage whiteBitmap = new BitmapImage();
            whiteBitmap.BeginInit();
            whiteBitmap.UriSource = new Uri(@pathToWhite, UriKind.RelativeOrAbsolute);
            whiteBitmap.EndInit();
            Image whiteImg = new Image();
            whiteImg.Source = whiteBitmap;

            textures.AddTexture(type, blackImg, whiteImg);
        }
    }
}
