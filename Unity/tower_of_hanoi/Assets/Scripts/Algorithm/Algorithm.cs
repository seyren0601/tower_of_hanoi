using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace tower_of_hanoi.Classes
{
    public class Algorithm:MonoBehaviour
    {
        public static List<(int, int)> Moves = new List<(int, int)>();
        /*public static List<State>? Solve_AStar(State start, State goal)
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
                for(int i = 0; i < State.NUM_OF_TOWER; i++)
                {
                    for(int j = 0; j < State.NUM_OF_TOWER; j++)
                    {
                        State? newState = null;
                        if (i != j)
                        {
                            newState = state.Move(i, j);
                        }
                        else continue;
                        if (newState is not null) // If move is valid
                        {
                            if (!Open_Check.ContainsKey(newState) && !Close.Any(x => x == newState)) // Skip state even if found in Open
                            {
                                Open_Check[newState] = newState.g;
                                Open.Enqueue(newState, newState);
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
        }*/

        public static void Solve_Recursion(int n, int from,
                             int to, int aux)
        {
            if (n == 0)
            {
                return;
            }
            Solve_Recursion(n - 1, from, aux, to);
            Moves.Add((from, to));
            Solve_Recursion(n - 1, aux, to, from);
        }
    }
}
