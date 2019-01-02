using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Othello_Board othello_Board = new Othello_Board();
            othello_Board.SetBoard();
            othello_Board.PrintBoard();
            bool can_take_moves = true; 
            while(can_take_moves)
            {
                othello_Board.PrintBoard();
                var moves = Othello_Minimax.GetBestMove(new Othello_Board(othello_Board), State.White, 5);
                if (moves.Item3 == -1)
                {
                    can_take_moves = false;
                }

                else
                {
                    othello_Board.UpdateBoard(moves.Item3, State.White, true);
                }

                var moves2 = Othello_Minimax.GetBestMove(new Othello_Board(othello_Board), State.Black, 1);
                if (moves2.Item3 == -1)
                {
                    can_take_moves = false;
                }

                else
                {
                    othello_Board.UpdateBoard(moves2.Item3, State.Black, true);
                    can_take_moves = true;
                }

                //var white_moves = othello_Board.PossibleMoves(State.White);
                //if(white_moves.Count > 0)
                //{
                //    othello_Board.UpdateBoard(white_moves[0], State.White, true);
                //}
                //else
                //{
                //    can_take_moves = false; 
                //}


                var score = othello_Board.ScoreGame();
                Console.WriteLine("\nBlack:\t" + score.Item1 + "\nWhite:\t" + score.Item2 + "\n"); 
            }
            int x = 2;
            Console.ReadKey();
        }

    }
}
