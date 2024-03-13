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
        // Các giá trị số lượng đĩa và cột để thử nghiệm
        public const int NUM_OF_TOWER = 3;
        public const int DISC_COUNT = 7;


        // Cấu trúc dữ liệu để lưu tình trạng các cột
        // Mỗi cột là một stack (thể hiện tính vào trước ra sau của đĩa)
        // Tất cả được lưu vào một array có capacity là 3 (3 cột)
        public Stack<int>[] towers { get; set; } = new Stack<int>[3];
        // g là số bước đi để đến được trạng thái hiện tại
        public int g { get; set; }
        // f = h (tính bằng hàm GetHeuristicValue())
        // Bỏ qua g trong f vì g có giá trị cố định (+1) khi di chuyển qua trạng thái mới
        // Hàm này trả về số lượng đĩa thiếu/sai vị trí trong cột đích
        public int f 
        { 
            get
            {
                return GetHeuristicValue();
            } 
        }
        // pre là một tuple chứa:
        // một tuple bao gồm 2 số nguyên: cột lấy đĩa và cột đặt đĩa (giá trị từ 0 đến 2)
        // một đối tượng State thể hiện trạng thái kế trước của trạng thái hiện tại
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
        // Hàm di chuyển với tham số là cột lấy đĩa (from) và cột đặt đĩa (to)
        // Hàm sẽ kiểm tra các điều kiện:
        //      + cột lấy đĩa (from) có đĩa hay không
        //      + cột đặt đĩa (to) có trống hay không
        //      + đĩa trên cùng (top) của cột đặt đĩa (to) có nhỏ hơn đĩa trên cùng (top) của cột lấy đĩa hay không (from)
        // Nếu đáp ứng điều kiện, hàm trả về một trạng thái mới sau khi di chuyển đĩa
        // Nếu không, hàm trả về null
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
        // Hàm in các cột trong trạng thái
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

        // Hàm clone một deep copy của trạng thái hiện tại (để tạo ra trạng thái mới)
        // Ý tưởng thuật toán là serialize đối tượng hiện tại và deserialize để lấy một đối tượng mới
        // Thuật toán cần phải thực hiện bước này 2 lần, vì mỗi lần thực hiện các stack của đối tượng sẽ đảo chiều
        // [Note] C# không cho phép pass by value cho các đối tượng thuộc kiểu phức tạp
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

        // Hàm tính giá trị heuristic
        int GetHeuristicValue() // Count the number of wrong ordered discs in goal
        {
            // Tính số đĩa nằm sai vị trí ở mỗi cột
            int wrong_discs_num_1 = CountWrongDiscs(towers[2]);
            int wrong_discs_num_2 = CountWrongDiscs(towers[1]);
            int wrong_discs_num_3 = CountWrongDiscs(towers[0]);
            // Heuristic bằng tổng số đĩa nằm sai vị trí ở mỗi cột
            return wrong_discs_num_1 + wrong_discs_num_2 + wrong_discs_num_3;
        }

        // Hàm tính số đĩa nằm sai vị trí trong 1 cột
        int CountWrongDiscs(Stack<int> tower)
        {
            // Tạo array tượng trưng cho trường hợp chuẩn của cột (số phần tử = tổng số đĩa)
            int[] array = new int[DISC_COUNT];
            // Copy số đĩa có trong cột vào array
            // => đĩa nhỏ nhất nằm ở vị trí đầu của mảng
            // => mảng được sắp xếp tăng dần
            // => nếu không đủ số lượng đĩa, các phần tử sau đó sẽ có giá trị default = 0
            tower.ToArray().CopyTo(array, 0);
            // Giá trị đĩa lớn nhất
            int max_disc = DISC_COUNT;
            int count = 0;
            // Lặp qua mảng từ chỉ số lớn nhất
            for (int i = DISC_COUNT - 1; i >= 0; i--)
            {
                // Nếu giá trị tại chỉ số bằng với giá trị lớn nhất hiện tại
                // => đĩa nằm đúng vị trí
                // => cộng biến count lên 1
                if (array[i] == max_disc)
                {
                    count += 1;
                }
                // Nếu phần tử hiện tại != 0 (phần tử đang xét không phải đĩa) => trừ giá trị lớn nhất đi 1
                // để tiếp tục kiểm tra
                if (array[i] != 0)
                {
                    max_disc -= 1;
                }
            }
            // Trả về số đĩa nằm sai vị trí = tổng số đĩa - số đĩa đúng vị trí
            return DISC_COUNT - count;
        }
        #endregion

        #region Operators
        // Overload operator == và != để nhận biết hai trạng thái giống nhau
        // dựa trên các stack (cột) trong hai trạng thái
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

    // Class comparer để sắp xếp các trạng thái trong PriorityQueue
    internal class StateComparer : IComparer<State>
    {
        public int Compare(State lhs, State rhs)
        {
            if(lhs.f == rhs.f) return lhs.g.CompareTo(rhs.g);
            return lhs.f.CompareTo(rhs.f);
        }
    }
}