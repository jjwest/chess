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
        private bool hasPressedSquare = false;
        private GameMoveEntity movement = new GameMoveEntity(new Entities.Point(0, 0), new Entities.Point(0, 0));
        private Textures textures;
        private GameLogic logic;
        private const int SQUARE_SIZE = 60;

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
            logic = new GameLogic(new Database.Database(), rules);
            var state = logic.GetInitialState();
            Draw(state);
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
        private void Draw(GameStateEntity state)
        {
            if (state.PawnIsPromoted)
            {
                DrawPromotion(state);
            }
            else
            {
                this.GameBoard.Children.Clear();
                DrawBoard(state);
                DrawGamePieces(state);
                DrawInvisibleButtons(state);
                DrawLabels(state);
            }    
        }

        private void DrawPromotion(GameStateEntity state)
        {
            DeactivateBoard(state);

            infoLabel.Content = "Your pawn has been promoted.\nPlease select a piece";
            var promotingPlayer = state.ActivePlayer == Entities.Color.White ? Entities.Color.Black : Entities.Color.White;

            var options = new List<GamePiece>
            {
                 new GamePiece(PieceType.Queen, promotingPlayer),
                  new GamePiece(PieceType.Rook, promotingPlayer),
                  new GamePiece(PieceType.Bishop, promotingPlayer),
                  new GamePiece(PieceType.Knight, promotingPlayer)
            };

            DrawPromotionOptions(options, state);
            DrawPromotionButtons(options);  
        }

        private void DeactivateBoard(GameStateEntity state)
        {
            var converter = new BrushConverter();
            var whiteBrush = (Brush)converter.ConvertFromString("#99F9D093");

            for (int y = 0; y < state.GameBoard.Width(); y++)
            {
                for (int x = 0; x < state.GameBoard.Width(); x++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Height = SQUARE_SIZE;
                    rect.Width = SQUARE_SIZE;
                    rect.Name = "rect" + x.ToString() + y.ToString();
                    rect.Fill = whiteBrush;
                    this.GameBoard.Children.Add(rect);
                    Grid.SetColumn(rect, x);
                    Grid.SetRow(rect, y);
                }
            }
        }

        private void DrawPromotionOptions(List<GamePiece> options, GameStateEntity state)
        {
           
            for (int i = 0; i < options.Count; i++)
            {
                var texture = textures.GetTexture(options[i]);
                this.promotionGrid.Children.Add(texture);
                Grid.SetColumn(texture, i);
                Grid.SetRow(texture, 0);
            }
        }

        private void DrawPromotionButtons(List<GamePiece> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                Button button = new Button();
                button.Width = SQUARE_SIZE;
                button.Height = SQUARE_SIZE;
                button.Name = options[i].Type.ToString();
                button.Opacity = 0;
                button.Click += HandlePromotion;

                this.promotionGrid.Children.Add(button);
                Grid.SetColumn(button, i);
                Grid.SetRow(button, 0);
            }
        }

        private void HandlePromotion(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var pieceType = button.Name;

            if (pieceType == "Queen")
                movement.Type = PieceType.Queen;
            else if (pieceType == "Rook")
                movement.Type = PieceType.Rook;
            else if (pieceType == "Knight")
                movement.Type = PieceType.Knight;
            else
                movement.Type = PieceType.Bishop;

            try
            {
                var newState = logic.TransformPiece(movement);
                this.promotionGrid.Children.Clear();
                Draw(newState);
            }
            catch (Exception ex)
            {
                DisplayError("An error has occurred. Please reinstall");
            }

        }

        private void DrawBoard(GameStateEntity state)
        {
            var converter = new BrushConverter();
            var blackBrush = (Brush)converter.ConvertFromString("#FF480000");
            var whiteBrush = (Brush)converter.ConvertFromString("#FFF9D093");

            for (int y = 0; y < state.GameBoard.Width(); y++)
            {
                for (int x = 0; x < state.GameBoard.Width(); x++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Height = SQUARE_SIZE;
                    rect.Width = SQUARE_SIZE;
                    rect.Name = "rect" + x.ToString() + y.ToString();

                    if ((x + y) % 2 == 0)
                        rect.Fill = blackBrush;
                    else
                        rect.Fill = whiteBrush;

                    this.GameBoard.Children.Add(rect);
                    Grid.SetColumn(rect, x);
                    Grid.SetRow(rect, y);
                }
            }
        }

        private void DrawGamePieces(GameStateEntity state)
        {
            for (int y = 0; y < state.GameBoard.Width(); y++)
            {
                for (int x = 0; x < state.GameBoard.Width(); x++)
                {
                    var piece = state.GameBoard.GetPieceAt(new Entities.Point(x, y));
                    if (piece.Type != PieceType.None)
                    {
                        var texture = textures.GetTexture(piece);
                        this.GameBoard.Children.Add(texture);
                        Grid.SetColumn(texture, x);
                        Grid.SetRow(texture, y);
                    }
                }
            }
        }

        private void DrawInvisibleButtons(GameStateEntity state)
        {
            for (int y = 0; y < state.GameBoard.Width(); y++)
            {
                for (int x = 0; x < state.GameBoard.Width(); x++)
                {
                    Button button = new Button();
                    button.Width = SQUARE_SIZE;
                    button.Height = SQUARE_SIZE;
                    button.Name = "button" + x.ToString() + y.ToString();
                    button.Opacity = 0;
                    button.Click += HandleClick;

                    this.GameBoard.Children.Add(button);
                    Grid.SetColumn(button, x);
                    Grid.SetRow(button, y);
                }
            }
        }

        private void DrawLabels(GameStateEntity state)
        { 
            if (state.ActivePlayer == Entities.Color.White)
                playerTurnLabel.Content = "White Player's Turn";
            else
                playerTurnLabel.Content = "Black Player's Turn";

            if (state.Winner != Entities.Color.None)
                infoLabel.Content = String.Format("Game is over, \n{0} player has won!", state.Winner);
            else if (state.KingIsChecked)
                infoLabel.Content = "You're checked!";
            else
                infoLabel.Content = "";
        }
        private void HandleClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            // Buttons are always named buttonXY, where X and Y are its coordinates
            int length = button.Name.Length;
            int x = (int)Char.GetNumericValue(button.Name[length - 2]);
            int y = (int)Char.GetNumericValue(button.Name[length - 1]);

            if (hasPressedSquare)
            {
                movement.RequestedPos = new Entities.Point(x, y);
                try
                {
                    var newState = logic.MovePiece(movement);
                    Draw(newState);
                    hasPressedSquare = false;
                }
                catch(Exception ex)
                {
                    DisplayError("An error has occurred. Please reinstall.");
                }            
            }
            else
            {
                hasPressedSquare = true;
                movement.CurrentPos = new Entities.Point(x, y);
            }
                
        }

        public void LoadTexture(PieceType type, string pathToBlack, string pathToWhite)
        {
            BitmapImage blackBitmap = new BitmapImage();
            blackBitmap.BeginInit();
            blackBitmap.UriSource = new Uri(@pathToBlack, UriKind.RelativeOrAbsolute);
            blackBitmap.EndInit();
           

            BitmapImage whiteBitmap = new BitmapImage();
            whiteBitmap.BeginInit();
            whiteBitmap.UriSource = new Uri(@pathToWhite, UriKind.RelativeOrAbsolute);
            whiteBitmap.EndInit();
         

            textures.AddTexture(type, blackBitmap, whiteBitmap);
        }

        private void QuitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StartNewGame(object sender, RoutedEventArgs e)
        {
            logic.ResetBoard();
            var state = logic.GetInitialState();
            Draw(state);
        }

        private void DisplayError(string message)
        {
            MessageBoxResult error = MessageBox.Show(message, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Error);
            if (error == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
