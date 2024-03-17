using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using tower_of_hanoi.Classes;

namespace tower_of_hanoi.Classes
{
    public class Algorithm:MonoBehaviour
    {
        public static List<(int, int)> Moves;
        public static async Task<List<State>>? Solve_AStar(State start, State goal)
        {
            if(start == goal) return null;
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
                            if (!Open_Check.ContainsKey(newState) && !Close.Any(x => x == newState)) // Only add to Open if state is not in Open and not in Close
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
        }

        public static async Task<List<(int, int)>> GetMoveList(List<State> Close){
            List<(int, int)> Moves = new List<(int, int)>();
            if(Close != null){
                State goal = Close.Last();
                do{
                    int from = goal.pre.Value.Item1.Item1;
                    int to = goal.pre.Value.Item1.Item2;
                    Moves.Add((from, to));
                    goal = goal.pre.Value.Item2;
                }while(goal.pre != null);
                Moves.Reverse();
            }
            return Moves;
        }

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
