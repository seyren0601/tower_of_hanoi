using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace tower_of_hanoi.Classes
{
    [Serializable]
    internal class State
    {
        public const int NUM_OF_TOWER = 3;
        public const int DISC_COUNT = 5;
        public Stack<int>[] towers { get; set; } = new Stack<int>[3];
        public int g { get; set; }
        public int f { get { return DISC_COUNT - towers[2].Count; } } 
        // Recommend: f = (check order of goal column)
        // + (check minimal moves required to get each disk out in current state)
        public ((int , int), State)? pre { get; set; }
        public State() { }
        public State(Stack<int>[] towers, int g)
        {
            towers.CopyTo(this.towers, 0);
            this.g = g;
        }

        public State(Stack<int>[] towers, int g, ((int, int), State)? pre) : this(towers, g)
        {
            this.pre = pre;
        }

        #region Move
        public State? Move(int from, int to)
        {
            State newState = this.Clone();
            if (newState.towers[from].TryPeek(out _))
            {
                int disc_move = newState.towers[from].Pop();
                if (!newState.towers[to].TryPeek(out _) || disc_move < newState.towers[to].Peek())
                {
                    newState.towers[to].Push(disc_move);
                    newState.pre = ((from, to), this);
                    newState.g += 1;
                    return new State(newState.towers, newState.g, newState.pre);
                }
            }
            return null;
        }
        #endregion

        #region Utilities
        public void PrintTowers()
        {
            State copy = this.Clone();
            for (int i = 0; i < NUM_OF_TOWER; i++)
            {
                Console.Write("{ ");
                while (copy.towers[i].Count > 0)
                {
                    Console.Write(copy.towers[i].Pop() + " ");
                }
                Console.Write("}");
            }
            Console.WriteLine($"({f})");
        }

        // Equals and GetHashCode methods, just in case it's needed
        /*public override bool Equals(object? obj)
        {
            return obj is State state &&
                   this == (State)obj;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(towers);
        }*/

        // Clone method to make a deep copy of current state
        public State Clone()
        {
            if (this.GetType().IsSerializable)
            {
                // Serialize twice to normalize the stack
                var json = JsonSerializer.Serialize(this);
                State state = JsonSerializer.Deserialize<State>(json);
                var json2 = JsonSerializer.Serialize(state);
                State state_final = JsonSerializer.Deserialize<State>(json2);
                return state_final;
            }
            return null;
        }
        #endregion

        #region Operators
        public static bool operator ==(State lhs, State rhs)
        {
            var lhs_clone = lhs.Clone();
            var rhs_clone = rhs.Clone();
            for(int i = 0; i < NUM_OF_TOWER; i++)
            {
                while (lhs_clone.towers[i].Count > 0 && rhs_clone.towers[i].Count > 0)
                {
                    int left = lhs_clone.towers[i].Pop();
                    int right = rhs_clone.towers[i].Pop();
                    if (left != right) return false;
                }
                if (lhs_clone.towers[i].TryPeek(out _) || rhs_clone.towers[i].TryPeek(out _)) return false;
            }
            return true;
        }

        public static bool operator !=(State lhs, State rhs)
        {
            return !(lhs == rhs);
        }

        #endregion
    }

    internal class StateComparer : IComparer<State>
    {
        public int Compare(State lhs, State rhs)
        {
            if (lhs.g == rhs.g) return lhs.f.CompareTo(rhs.f);
            return lhs.g.CompareTo(rhs.g);
        }
    }
}