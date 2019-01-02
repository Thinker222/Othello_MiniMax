using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Bot
{
    public class Othello_Board
    {
        List<State> board;

        public Othello_Board()
        {
            board = new List<State>(64);
            for (int i = 0; i < 64; i++)
            {
                board.Add(State.Empty);
            }
        }

        public Othello_Board(Othello_Board orig_board)
        {
            board = new List<State>(orig_board.board);
        }

        public List<State> GetBoardStates()
         {
            return board;
         }

        public void SetBoard()
        {
            board[(3 * 8) + 3] = State.White;
            board[(3 * 8) + 4] = State.Black;
            board[(4 * 8) + 3] = State.Black;
            board[(4 * 8) + 4] = State.White;
        }

        public State GetState(int position, Direction dir)
        {
            int row = position / 8;
            int column = position % 8;
            switch (dir)
            {
                case Direction.Up_Left:
                    row++;
                    column--;
                    break;
                case Direction.Up:
                    row++;
                    break;
                case Direction.Up_Right:
                    row++;
                    column++;
                    break;
                case Direction.Right:
                    column++;
                    break;
                case Direction.Down_Right:
                    row--;
                    column++;
                    break;
                case Direction.Down:
                    row--;
                    break;
                case Direction.Down_Left:
                    row--;
                    column--;
                    break;
                case Direction.Left:
                    column--;
                    break;
                
            }
            if(column < 0 || row < 0 || column > 7 || row > 7)
            {
                return State.NonExist;
            }
            else
            {
                return board[(8 * row) + column];
            }
        }

        public int GetPos(int position, Direction dir)
        {
            int row = position / 8;
            int column = position % 8;
            switch (dir)
            {
                case Direction.Up_Left:
                    row++;
                    column--;
                    break;
                case Direction.Up:
                    row++;
                    break;
                case Direction.Up_Right:
                    row++;
                    column++;
                    break;
                case Direction.Right:
                    column++;
                    break;
                case Direction.Down_Right:
                    row--;
                    column++;
                    break;
                case Direction.Down:
                    row--;
                    break;
                case Direction.Down_Left:
                    row--;
                    column--;
                    break;
                case Direction.Left:
                    column--;
                    break;
            }
            if (column < 0 || row < 0 || column > 7 || row > 7)
            {
                return -1;
            }
            else
            {
                return (8 * row) + column;
            }
        }

        public bool UpdateBoard(int position, State state, bool flip)
        {
            if(board[position] != State.Empty)
            {
                return false; 
            }


           bool flipped_some_pieces1 =  FlipPiecesInDirection(position, state, Direction.Up_Left, flip);
           bool flipped_some_pieces2 =  FlipPiecesInDirection(position, state, Direction.Up, flip);
           bool flipped_some_pieces3 =  FlipPiecesInDirection(position, state, Direction.Up_Right, flip);
           bool flipped_some_pieces4 =  FlipPiecesInDirection(position, state, Direction.Right, flip);
           bool flipped_some_pieces5 =  FlipPiecesInDirection(position, state, Direction.Down_Right, flip);
           bool flipped_some_pieces6 =  FlipPiecesInDirection(position, state, Direction.Down, flip);
           bool flipped_some_pieces7 =  FlipPiecesInDirection(position, state, Direction.Down_Left, flip);
            bool flipped_some_pieces8 =  FlipPiecesInDirection(position, state, Direction.Left, flip);
            return flipped_some_pieces1 || flipped_some_pieces2 || flipped_some_pieces3 || flipped_some_pieces4 || flipped_some_pieces5 || flipped_some_pieces6 
                || flipped_some_pieces7 || flipped_some_pieces8 ;

        }

        public bool FlipPiecesInDirection(int orig_position, State state, Direction dir, bool flip)
        {
            int position = orig_position; 
            bool flipped_a_piece = false;
            State enemy_value = State.NonExist;
            // Deciede enemy state
            if (state == State.Black)
            {
                enemy_value = State.White;
            }
            else
            {
                enemy_value = State.Black;
            }

            do
            {
                position = GetPos(position, dir);
                if(position == -1)
                {
                    return flipped_a_piece;
                }
            } while (board[position] == enemy_value);

            if(board[position] == state)
            {
                position = orig_position;
                do
                {
                    if (board[position] == enemy_value)
                    {
                        if (flip)
                        {
                            board[position] = state;
                        }
                        flipped_a_piece = true;
                    }
                    position = GetPos(position, dir);
                } while (board[position] == enemy_value);
            }
            if(flipped_a_piece && flip)
            {
                board[orig_position] = state;
            }
            return flipped_a_piece;
        }

        public List<int> PossibleMoves(State state)
        {
            List<int> possible_moves = new List<int>(64);
            for(int i = 0; i < 64; i++)
            {
                if(UpdateBoard(i, state, false))
                {
                    possible_moves.Add(i);
                }
            }
            return possible_moves; 
        }

       public Tuple<int, int> ScoreGame()
        {
            int black = 0;
            int white = 0;
            for(int i = 0; i < 64; i++)
            {

                if (board[i] == State.Black)
                {
                    black++;
                }
                if (board[i] == State.White)
                {
                    white++;
                }
            }
            return new Tuple<int, int>(black, white);
        }

        public Tuple<int, int> EvaluateGameState()
        {
            int corner_val = 5;
            int edge_val = 2;
            int reg_val = 1;
            int black = 0;
            int white = 0; 

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    int temp_val = 0;
                    if (IsCorner(i, j))
                    {
                        temp_val = corner_val;
                    }
                    else if(IsEdge(i,j))
                    {
                        temp_val = edge_val;
                    }
                    else
                    {
                        temp_val = reg_val;
                    }

                    if (board[i * 8 + j] == State.White)
                    {
                        white += temp_val; 
                    }
                    else if(board[i * 8 + j] == State.Black)
                    {
                        black += temp_val;
                    }
                }
            }

            return new Tuple<int, int>(black, white);
        }

        public bool IsCorner(int row, int column)
        {
            if (row == 0 || row == 7)
            {
                if (column == 0 || column == 7)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEdge(int row, int column)
        {
            if(row == 0 || row == 7 || column == 0 || column == 7)
            {
                return true; 
            }
            else
            {
                return false; 
            }
        }

        public void PrintBoard()
        {
            for(int i = 0; i < 64; i++)
            {
                if (i % 8 == 0)
                {
                    Console.WriteLine();
                }
                if(board[i] == State.Black)
                {
                    Console.Write("  B  ");
                }

                else if(board[i] == State.White)
                {
                    Console.Write("  W  ");
                }

                else if(board[i] == State.Empty)
                {
                    Console.Write("  0  ");
                }

                else
                {
                    Console.Write("  ?  ");
                }
            }
        }
    }
}
