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
Random rand = new Random();
for(int i = State.DISC_COUNT; i > 0; i--)
{
    tower_init[rand.Next() % 3].Push(i);
}

State init = new State(tower_init, 0); // Create start state

// Define goal states
int intermediate_goal;
for (int i = 0; i < State.NUM_OF_TOWER; i++)
{
    int[] tower = init.towers[i].ToArray();
    if (tower[tower.Length - 1] == State.DISC_COUNT - 1)
    {
        intermediate_goal = i;
        break;
    }
}
// all discs in 3rd column
Stack<int>[] tower_goal1 = { new Stack<int>(), new Stack<int>(), new Stack<int>() };
st = new Stack<int>();
for (int i = State.DISC_COUNT; i > 0; i--)
{
    st.Push(i);
}
tower_goal1[2] = st;

// all discs in 2nd column
Stack<int>[] tower_goal2 = { new Stack<int>(), new Stack<int>(), new Stack<int>() };
tower_goal2[1] = st;

// all discs in 3rd column
Stack<int>[] tower_goal3 = { new Stack<int>(), new Stack<int>(), new Stack<int>() };
tower_goal3[0] = st;

List<State> goals = [ new State(tower_goal1, 0), new State(tower_goal2, 0), new State(tower_goal3, 0) ];

System.Timers.Timer timer = new System.Timers.Timer(1);
int miliseconds = 0;
timer.Elapsed += async (sender, e) => miliseconds += 1;
timer.Start();
List<State>? Close = Algorithm.Solve_AStar(init, goals); // Get list of moves checked (include path to goal)
timer.Stop();

Console.WriteLine("Start state: ");
init.PrintTowers();


// Solution with recursion
//System.Timers.Timer timer = new System.Timers.Timer(1);
//int miliseconds = 0;
timer.Elapsed += async (sender, e) => miliseconds += 1;
State intermediate = Close!.Last();
int start, aux, goal;
if (intermediate.towers[0].Count > 0)
{
    start = 0; aux = 1; goal = 2;
    Console.WriteLine("Start state after A*: ");
    intermediate.PrintTowers();
    RecursionPrintPath(start, aux, goal);
}
else if (intermediate.towers[1].Count > 0)
{
    start = 1; aux = 0; goal = 3;
    Console.WriteLine("Start state after A*: ");
    intermediate.PrintTowers();
    RecursionPrintPath(start, aux, goal);
}
else
{
    AStartPrintPath(Close);
}

void AStartPrintPath(List<State> Close)
{
    Console.WriteLine($"Algorithm runs in {miliseconds} miliseconds");
    Console.WriteLine($"Total nodes checked: {Close.Count}\n");
    int count = 0;
    State end = Close.Last();
    List<State> path = new List<State>();
    while (end.pre != null)
    {
        path.Add(end);
        end = end.pre.Value.Item2;
        count += 1;
    }
    path.Add(init);
    path.Reverse();
    Console.WriteLine($"Total moves: {count}");
    foreach (State state in path)
    {
        state.PrintTowers();
        if (state.pre != null)
            Console.WriteLine($"Move = " + state.pre.Value.Item1.Item1 + " to " + state.pre.Value.Item1.Item2 + "\n");
    }
}

void RecursionPrintPath(int start, int aux, int goal)
{
    Algorithm.Solve_Recursion(State.DISC_COUNT, start, goal, aux);
    Console.WriteLine($"A* runs for {miliseconds} miliseconds");
    Console.WriteLine("Moves: " + Algorithm.Moves.Count);
    foreach (var move in Algorithm.Moves)
    {
        Console.WriteLine(move.Item1 + " to " + move.Item2);
    }
}