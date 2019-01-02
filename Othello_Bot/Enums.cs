using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Bot
{
public enum State
    {
        NonExist = -1,
        Empty = 0, 
        White = 1, 
        Black = 2
    }
   public class StateHandler
    {
        public static State GetOppositeState(State state)
        {
            if (state == State.Black)
            {
                return State.White;
            }

            else if (state == State.White)
            {
                return State.Black;
            }

            else
            {
                return State.NonExist;
            }
        }
    }

public enum Direction
    {
        Neutral = -1,
        Up_Left = 0, 
        Up = 1,
        Up_Right = 2,
        Right = 3, 
        Down_Right = 4, 
        Down = 5, 
        Down_Left = 6, 
        Left = 7
    }
}
