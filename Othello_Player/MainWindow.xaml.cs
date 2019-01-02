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
using System.Threading;
using Othello_Bot;
namespace Othello_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Othello_Board board;
        State current_state; 
        public MainWindow()
        {
      
            InitializeComponent();
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Text = (i * 8 + j).ToString();
                    BetterButton button = new BetterButton();
                    button.Background = Brushes.Transparent;
                    button.ellipse = new Ellipse();
                    button.ellipse.Fill = Brushes.Green;
                    button.row = i;
                    button.column = j;
                    button.Background = Brushes.Red;
                    button.Background = Brushes.Transparent;
                    button.Click += new RoutedEventHandler(UpdateButton);
                    Grid.Children.Add(button.ellipse);
                    Grid.SetRow(button.ellipse, i);
                    Grid.SetColumn(button.ellipse, j);
                    Grid.Children.Add(button);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    board = new Othello_Board();
                    board.SetBoard();
                    current_state = State.Black;
                    UpdateViewableBoard();
                }
            }
        }

        protected void UpdateButton(object sender, EventArgs e)
        {
            if (current_state == State.White)
            {
                var moves = Othello_Minimax.GetBestMove(new Othello_Board(board), State.White, 4);
                if (moves.Item3 == -1)
                {
                    this.current_state = StateHandler.GetOppositeState(current_state);
                    return;
                }

                else
                {
                    board.UpdateBoard(moves.Item3, State.White, true);
                }
                //BetterButton btn = (BetterButton)sender;

                //int row = btn.row;
                //int col = btn.column;
                //board.UpdateBoard(row * 8 + col, State.White, true);
            }
            else
            {
                var moves = Othello_Minimax.GetBestMove(new Othello_Board(board), State.Black, 2);
                if (moves.Item3 == -1)
                {
                    this.current_state = StateHandler.GetOppositeState(current_state);
                    return;
                }

                else
                {
                    board.UpdateBoard(moves.Item3, State.Black, true);
                }
            }
            this.current_state = StateHandler.GetOppositeState(current_state);
            UpdateViewableBoard();
        }

        protected void UpdateViewableBoard()
        {
            var buttons = Grid.Children.OfType<Button>();
            foreach(BetterButton button in buttons)
            {
                State state = board.GetState(button.row * 8 + button.column, Direction.Neutral);
                if(state == State.Empty)
                {
                    button.ellipse.Fill = Brushes.Green;
                }

                else if (state == State.White)
                {
                    button.ellipse.Fill = Brushes.White;
                }

                else if (state == State.Black)
                {
                    button.ellipse.Fill = Brushes.Black;
                }

                else
                {
                    button.ellipse.Fill = Brushes.Transparent;
                }
            }
        }
    }
}
