// See https://aka.ms/new-console-template for more information
using System.Collections.Specialized;
using System.Resources;
using tower_of_hanoi.Classes;
using System;
using System.Threading.Tasks;
using System.Timers;


// Solution with A*
// Define start state
Stack<int>[] tower_init = { new Stack<int>(), new Stack<int>(), new Stack<int>()};
Stack<int> st = new Stack<int>();
for(int i = State.DISC_COUNT; i > 0; i--)
{
    st.Push(i);
}
tower_init[0] = st;

// Define goal state
Stack<int>[] tower_goal = { new Stack<int>(), new Stack<int>(), new Stack<int>() };
st = new Stack<int>();
for (int i = State.DISC_COUNT; i > 0; i--)
{
    st.Push(i);
}
tower_goal[2] = st;


State init = new State(tower_init, 0); // Create start state
State goal = new State(tower_goal, 0); // Create goal state

System.Timers.Timer timer = new System.Timers.Timer(1);
int miliseconds = 0;
timer.Elapsed += async (sender, e) => miliseconds += 1;
timer.Start();
List<State>? Close = Algorithm.Solve_AStar(init, goal); // Get list of moves checked (include path to goal)
timer.Stop();
Console.WriteLine($"Algorithm runs in {miliseconds} miliseconds");
if(Close != null)
{
    Console.WriteLine($"Total nodes checked: {Close.Count}\n");
    int count = 0;
    State end = Close.Last();
    List<State> path = new List<State>();
    while(end.pre != null)
    {
        path.Add(end);
        end = end.pre.Value.Item2;
        count += 1;
    }
    path.Add(init);
    path.Reverse();
    Console.WriteLine($"Total moves: {count}");
    foreach(State state in path)
    {
        state.PrintTowers();
        if(state.pre != null)
            Console.WriteLine($"Move = " + state.pre.Value.Item1.Item1 + " to " + state.pre.Value.Item1.Item2 + "\n");
    }
}
else
{
    Console.WriteLine("path not found");
}


// Solution with recursion
/*
System.Timers.Timer timer = new System.Timers.Timer(1);
int miliseconds = 0;
timer.Elapsed += async (sender, e) => miliseconds += 1;
timer.Start();
Algorithm.Solve_Recursion(State.DISC_COUNT, 0, 2, 1);
timer.Stop();
Console.WriteLine($"Algorithm runs in {miliseconds} miliseconds");
Console.WriteLine("Moves: " + Algorithm.Moves.Count);
foreach(var move in Algorithm.Moves)
{
    Console.WriteLine(move.Item1 + " to " +  move.Item2);
}*/