using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace tower_of_hanoi.Classes
{
    static internal class Algorithm
    {
        // List lưu lại các bước đi dành cho thuật toán đệ quy
        // số đầu tiên trong tuple là cột lấy đĩa (from), số thứ hai là cột đặt đĩa (to)
        public static List<(int, int)> Moves = new List<(int, int)>();

        // Hàm giải bài toán Tháp Hà Nội bằng thuật giải A*
        public static List<State>? Solve_AStar(State start, List<State> goals)
        {
            int intermediate_goal;
            for(int i = 0; i < State.NUM_OF_TOWER; i++)
            {
                int[] tower = start.towers[i].ToArray();
                if (tower[tower.Length - 1] == State.DISC_COUNT - 1)
                {
                    intermediate_goal = i;
                    break;
                }
            }
            // Open là một PriorityQueue, với độ ưu tiên là f
            PriorityQueue<State, State> Open = new PriorityQueue<State, State>(new StateComparer());

            // Dictionary/Map để giảm độ phức tạp thao tác tìm kiếm trạng thái trong Open còn O(1)
            Dictionary<State, int> Open_Check = new Dictionary<State, int>();

            // Close là List lưu lại các trạng thái đã kiểm tra
            List<State> Close = new List<State>();

            // Bước đầu, enqueue trạng thái start vào Priorityqueue và khởi tạo key-value của start trong Open_Check
            Open.Enqueue(start, start);
            Open_Check[start] = start.g;

            while(Open.Count > 0)
            {
                // Lấy trạng thái (state) có priority cao nhất (giá trị f nhỏ nhất) trong PriorityQueue
                State state = Open.Dequeue();

                // Nếu trong Close chưa có trạng thái đang xét => thêm trạng thái vào Close
                // và xóa key-value của trạng thái trong Open_Check
                if (!Close.Any(x=>x == state))
                {
                    Close.Add(state);
                    Open_Check.Remove(state);
                }
                else // Nếu trạng thái đã có trong Close, bỏ qua
                {
                    continue;
                }

                if (goals.Any(x => x == state)) // Nếu trạng thái là trạng thái đích, trả về Close
                {
                    return Close;
                }

                // Xét từng tổ hợp bước đi của trạng thái hiện tại
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
                        // Trong trường hợp bước đi hợp lệ
                        if (newState is not null)
                        {
                            // Thêm trạng thái vào Open nếu trạng thái chưa có trong Open và Close
                            // Bỏ qua trường hợp trạng thái đã có trong Open, vì chắc chắn g của trạng thái đang xét
                            // sẽ lớn hơn hoặc bằng g của trạng thái trong Open
                            if (!Open_Check.ContainsKey(newState) && !Close.Any(x => x == newState)) // Only add to Open if state is not in Open and not in Close
                            {
                                Open_Check[newState] = newState.g;
                                Open.Enqueue(newState, newState);
                            }
                        }
                    }
                }
            }
            // Trả về null nếu không tìm được đường đi đến trạng thái đích
            return null;
        }

        // Hàm giải bài toán Tháp Hà Nội bằng thuật giải đệ quy
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
