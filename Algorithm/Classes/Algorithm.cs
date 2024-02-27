using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace tower_of_hanoi.Classes
{
    static internal class Algorithm
    {
        public static List<State>? Solve(State start, State goal)
        {
            bool path_found = false;
            PriorityQueue<State, State> Open = new PriorityQueue<State, State>(new StateComparer());
            Dictionary<State, int> Open_Check = new Dictionary<State, int>();
            List<State> Close = new List<State>();
            Open.Enqueue(start, start);
            Open_Check[start] = start.g;
            while(Open.Count > 0)
            {
                State state = Open.Dequeue();

                if (!Close.Any(x=>x == state))
                {
                    Close.Add(state);
                    Open_Check.Remove(state);
                }
                else
                {
                    continue;
                }

                if(state == goal)
                {
                    path_found = true;
                    return Close;
                }

                // For each moves from the current state
                foreach(var function in state.Moves)
                {
                    State? newState = function();
                    if(newState is not null) // If move is valid
                    {
                        if (!Open_Check.ContainsKey(newState) && !Close.Any(x=>x == newState))
                        {
                            Open_Check[newState] = newState.g;
                            Open.Enqueue(newState, newState);
                        }
                        else
                        {
                            if (Open_Check.ContainsKey(newState) && Open_Check[state] > newState.g)
                            {
                                Open.Enqueue(newState, newState);
                                Open_Check[newState] = newState.g;
                            }
                        }
                    }
                }
            }

            if (path_found)
            {
                return Close;
            }
            return null;
        }
    }
}
