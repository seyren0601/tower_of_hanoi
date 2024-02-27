// See https://aka.ms/new-console-template for more information
using System.Resources;
using tower_of_hanoi.Classes;

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

List<State>? Close = Algorithm.Solve(init, goal); // Get list of moves checked (include path to goal)
if(Close != null)
{
    Console.WriteLine($"Total nodes checked: {Close.Count}\n");
    State end = Close.Last();
    List<State> path = new List<State>();
    while(end.pre != null)
    {
        path.Add(end);
        end = end.pre.Value.Item2;
    }
    path.Add(init);
    path.Reverse();
    foreach(State state in path)
    {
        state.PrintTowers();
        if(state.pre != null)
            Console.WriteLine($"Move = " + state.pre.Value.Item1.Method.Name + "\n");
    }
}
else
{
    Console.WriteLine("path not found");
}