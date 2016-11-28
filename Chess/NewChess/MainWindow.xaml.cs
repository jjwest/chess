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
        public MainWindow()
        {
            InitializeComponent();
            RuleBook rules = LoadRules();
            GameLogic logic = new GameLogic(new Database.Database(), rules);
            var state = logic.GetInitialState();
            DrawGameBoard(state);
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
            var converter = new BrushConverter();
            var blackBrush = (Brush)converter.ConvertFromString("#FF480000");
            var whiteBrush = (Brush)converter.ConvertFromString("#FFF9D093");


            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Height = 60;
                    rect.Width = 60;

                    if ((x + y) % 2 == 0)
                    {
                        rect.Fill = blackBrush;
                    }
                    else
                    {
                        rect.Fill = whiteBrush;
                    }

                    this.GameBoard.Children.Add(rect);
                    Grid.SetColumn(rect, x);
                    Grid.SetRow(rect, y);
                }
            }


        }
    }
}
