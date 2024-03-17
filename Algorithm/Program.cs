// See https://aka.ms/new-console-template for more information
using System.Collections.Specialized;
using System.Resources;
using tower_of_hanoi.Classes;
using System;
using System.Threading.Tasks;
using System.Timers;

long total_time = 0;
int total_nodes = 0;
int instances = 0;
const int RUN_COUNT = 1000;
long max_time = long.MinValue;
long max_nodes = long.MinValue;
State worst_case = null;
Parallel.For(0, RUN_COUNT, number =>
{
    // Solution with A*
    // Define start state
    Stack<int>[] tower_init = { new Stack<int>(), new Stack<int>(), new Stack<int>() };
    Stack<int> st = new Stack<int>();
    Random rand = new Random();
    for (int i = State.DISC_COUNT; i > 0; i--)
    {
        tower_init[rand.Next() % 3].Push(i);
    }

    State init = new State(tower_init, 0); // Create start state

    //Define goal state
    for (int i = 0; i < State.NUM_OF_TOWER; i++)
    {
        int[] tower = init.towers[i].ToArray();
        if (tower.Length > 0 && tower[tower.Length - 1] == State.DISC_COUNT)
        {
            State.goal_tower = i;
            break;
        }
    }
    //State.goal_tower = 2;

    // all discs in goal column
    Stack<int>[] tower_goal = { new Stack<int>(), new Stack<int>(), new Stack<int>() };
    st = new Stack<int>();
    for (int i = State.DISC_COUNT; i > 0; i--)
    {
        st.Push(i);
    }
    tower_goal[State.goal_tower] = st;

    State goal_state = new State(tower_goal, 0);

    var watch = System.Diagnostics.Stopwatch.StartNew();
    List<State>? Close = Algorithm.Solve_AStar(init, goal_state); // Get list of moves checked (include path to goal)
    watch.Stop();
    total_time += watch.ElapsedMilliseconds;
    instances += 1;
    total_nodes += Close.Count;
    if (watch.ElapsedMilliseconds > max_time) max_time = watch.ElapsedMilliseconds;
    if (Close.Count > max_nodes)
    {
        worst_case = init;
        max_nodes = Close.Count;
    }
    Console.Clear();
    Console.WriteLine($"Done {instances}/{RUN_COUNT} instances");
});

Console.WriteLine($"Average A* runtime: {(float)total_time / RUN_COUNT}");
Console.WriteLine($"Average nodes checked: {(float)total_nodes / RUN_COUNT}");
Console.WriteLine($"Max time: {max_time}");
Console.WriteLine($"Max nodes: {max_nodes}");
Console.Write($"Worst case: "); worst_case.PrintTowers();


/*
// Print stats
Console.WriteLine("Start state: ");
init.PrintTowers();

// Solution with recursion
//System.Timers.Timer timer = new System.Timers.Timer(1);
//int miliseconds = 0;
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
    start = 1; aux = 0; goal = 2;
    Console.WriteLine("Start state after A*: ");
    intermediate.PrintTowers();
    RecursionPrintPath(start, aux, goal);
}
else
{
    AStartPrintPath(Close);
}*/

void AStartPrintPath(List<State> Close)
{
    //Console.WriteLine($"Algorithm runs in {watch.ElapsedMilliseconds} miliseconds");
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
    //path.Add(init);
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
    //Console.WriteLine($"A* runs for {watch.ElapsedMilliseconds} miliseconds");
   // Console.WriteLine($"Total nodes checked: {Close.Count}\n");
    Console.WriteLine("Moves: " + Algorithm.Moves.Count);
    foreach (var move in Algorithm.Moves)
    {
        Console.WriteLine(move.Item1 + " to " + move.Item2);
    }
}