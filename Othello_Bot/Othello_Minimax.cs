using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Bot
{
    public class Othello_Minimax
    {
       public class EvaluationParameters
        {
            int percent_threshold;
            int depth;
            double my_pieces_weight;
            double enemy_pieces_weight;
            double my_piece_minus_enemy_piece_weight;
            double my_corner_weight;
            double enemy_corner_weight;
            double my_sideline_weight;
            double enemy_sideline_weight;

            public static List<EvaluationParameters> GenerateAllParams(Random rnd)
            {
                int early_value = rnd.Next(1, 101);
                int mid_value = 0;
                int late_value = 0;
                do
                {
                    mid_value = rnd.Next(1, 101);
                } while (mid_value == early_value);
                do
                {
                    late_value = rnd.Next(1, 101);
                } while (late_value == mid_value || late_value == early_value);
                var values = new List<int>();
                values.Add(early_value);
                values.Add(mid_value);
                values.Add(late_value);
                values.Sort();
                var eval_params = new List<EvaluationParameters>();
                eval_params.Add(GenerateParams(values[0], rnd));
                eval_params.Add(GenerateParams(values[1], rnd));
                eval_params.Add(GenerateParams(values[2], rnd));
                return eval_params;
            }

            public static EvaluationParameters GenerateParams(int percent_threshold, Random rnd)
            {
                EvaluationParameters eval = new EvaluationParameters();
                eval.enemy_corner_weight = GetParameterDouble(rnd);
                eval.enemy_pieces_weight = GetParameterDouble(rnd);
                eval.enemy_sideline_weight = GetParameterDouble(rnd);
                eval.my_corner_weight = GetParameterDouble(rnd);
                eval.my_pieces_weight = GetParameterDouble(rnd);
                eval.my_piece_minus_enemy_piece_weight = GetParameterDouble(rnd);
                eval.my_sideline_weight = GetParameterDouble(rnd);
                eval.percent_threshold = percent_threshold;
                eval.depth = rnd.Next(1, 5);
                return eval;
            }

            public static double GetParameterDouble(Random rnd)
            {
                return (rnd.NextDouble() * 2) - 1;
            }
        }
        State my_state;
        int my_depth;
        EvaluationParameters early, mid, late;

       public Othello_Minimax(State state, Random rnd,List<EvaluationParameters> eval_params = null)
        {
            this.my_state = state; 
            if(eval_params == null)
            {
                eval_params = EvaluationParameters.GenerateAllParams(rnd);
            }
            early = eval_params[0];
            mid = eval_params[1];
            late = eval_params[2];
        }

        public Tuple<int, int, int> GetBestMove(Othello_Board board)
        {
            return GetBestMove(board, my_state, my_depth, -1);
        }

        public static Tuple<int, int, int> GetBestMove(Othello_Board board, State state, int depth, int last_move = -1)
        {
            // Make a copy of the board
            Othello_Board board_copy = new Othello_Board(board);
            // If there is a move execute it
            if(last_move != - 1)
            {
                board_copy.UpdateBoard(last_move, StateHandler.GetOppositeState(state), true);
              //  Console.WriteLine("PRinting board");
              //  board_copy.PrintBoard();
            }
            var possible_moves = board_copy.PossibleMoves(state);
            Tuple<int, int, int> current_best = new Tuple<int, int, int>(0, 0, 0);
            int best_value = int.MinValue;
            int best_move = -1;
            if(possible_moves.Count > 0 && depth > 0)
            {
                foreach(int move in possible_moves)
                {
                    Tuple<int, int, int> temp_best = GetBestMove(new Othello_Board(board_copy), StateHandler.GetOppositeState(state), depth - 1, move);
                    int temp_value = 0;
                    if (state == State.Black)
                    {
                        temp_value = temp_best.Item1 - temp_best.Item2;
                    }
                    else
                    {
                        temp_value = temp_best.Item2 - temp_best.Item1;
                    }
                    if(temp_value > best_value)
                    {
                        best_value = temp_value;
                        current_best = temp_best;
                        best_move = move;
                    }
                }
                return new Tuple<int, int, int>(current_best.Item1, current_best.Item2, best_move);
            }

            else
            {
                var score = board_copy.EvaluateGameState();
                //if(score.Item1 + score.Item2 > 50)
                //{
                //    score = 
                //}
                return new Tuple<int, int, int>(score.Item1, score.Item2, last_move);    
            }
            
        }
    }
}