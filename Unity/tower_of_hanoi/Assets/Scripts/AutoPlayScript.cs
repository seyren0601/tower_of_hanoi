using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using tower_of_hanoi.Classes;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoPlayScript:MonoBehaviour
{
    bool solved = false;
    Vector2 cot1;
    Vector2 cot2;
    Vector2 cot3;
    Stack<GameObject> stack_cot1;
    Stack<GameObject> stack_cot2;
    Stack<GameObject> stack_cot3;
    float disc_count;

    bool algorithm_init = false;
    Stack<GameObject>[] cot_list = new Stack<GameObject>[3];
    Stack<int>[] cot_int_init = {new Stack<int>(), new Stack<int>(), new Stack<int>()};
    Stack<int>[] cot_int_goal = {new Stack<int>(), new Stack<int>(), new Stack<int>()};
    List<(int, int)> Moves = new List<(int, int)>();
    
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    bool handled;
    async void Update()
    {
        if(GameInfo.May_play)
        {
            if (!algorithm_init)
            {
                cot1 = GameInfo.toado_cot1;
                cot2 = GameInfo.toado_cot2;
                cot3 = GameInfo.toado_cot3;
                stack_cot1 = GameInfo.cot1;
                stack_cot2 = GameInfo.cot2;
                stack_cot3 = GameInfo.cot3;
                disc_count = GameInfo.disc_count;
                cot_list[0] = stack_cot1;
                cot_list[1] = stack_cot2;
                cot_list[2] = stack_cot3;
                handled = false;
                solved = false;
                algorithm_init = false;
                
                // Get list bước đi bằng thuật toán đệ quy
                Algorithm.Moves = new List<(int, int)>();
                Algorithm.Solve_Recursion((int)disc_count, 0, 2, 1);
                Moves = Algorithm.Moves;

                // Get list bước đi bằng thuật toán A*
                /*cot_int_init[0] = GameInfo.cot1_int; // Khởi tạo trạng thái bắt đầu
                cot_int_init[1] = GameInfo.cot2_int;
                cot_int_init[2] = GameInfo.cot3_int;
                State start = new State(cot_int_init, 0);

                for(int i=(int)GameInfo.disc_count;i>0;i--){ // Khởi tạo trạng thái kết thúc
                    cot_int_goal[2].Push(i);
                }
                State goal = new State(cot_int_goal, 0);
                Moves = await Algorithm.GetMoveList(await Algorithm.Solve_AStar(start, goal));*/

                //Kết thúc khởi tạo và chạy thuật toán
                Debug.Log(Moves.Count);
                algorithm_init=true;
            }
            // 
            if(!solved && algorithm_init && !handled){
                StartCoroutine(AutoSolve());
            }
        }
        
    }

    IEnumerator MoveDisc(int from, int to)
    {

        GameObject obj_from = cot_list[from].Pop();
        Vector2 toado_to = (to==0)?cot1:(to==1)?cot2:cot3;
        float distanceX = Math.Abs(obj_from.transform.position.x - toado_to.x);
        float distanceY = Math.Abs(obj_from.transform.position.y - toado_to.y);
        cot_list[to].Push(obj_from);
        obj_from.GetComponent<Rigidbody2D>().isKinematic = true;
        StartCoroutine(animationY(obj_from, cot1.y));
        yield return new WaitForSeconds(distanceY/20f + 0.1f);
        StartCoroutine(animationX(obj_from, (to==0)?cot1.x:(to==1)?cot2.x:cot3.x));
        yield return new WaitForSeconds(distanceX/20f + 0.1f);
        obj_from.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    IEnumerator animationY(GameObject obj,float y){
        while(obj.transform.position.y < y){
            obj.transform.position += new Vector3(0, 0.1f, 0);
            yield return new WaitForSeconds(0.005f);
        }
    }

    IEnumerator animationX(GameObject obj, float x){
        if(obj.transform.position.x < x){
            while(obj.transform.position.x < x){
                obj.transform.position += new Vector3(0.1f , 0, 0);
                yield return new WaitForSeconds(0.005f);
            }
        }
        else{
            while(obj.transform.position.x > x){
                obj.transform.position += new Vector3(-0.1f , 0, 0);
                yield return new WaitForSeconds(0.005f);
            }
        }
        if(obj.transform.position.x < x) obj.transform.position = new Vector2(x, obj.transform.position.y);
        else obj.transform.position = new Vector2(x, obj.transform.position.y);
        
    }

    IEnumerator AutoSolve()
    {
        handled = true;
        for (int i = 0; i < Moves.Count; i++)
        {
            (int, int) move = Moves[i];
            StartCoroutine(MoveDisc(move.Item1, move.Item2));
            yield return new WaitForSecondsRealtime(2);
        }
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(3);
    }

    void NextMove(){
        if(!solved){
            MoveDisc(Moves[0].Item1, Moves[0].Item2);
        }
        if(Moves.Count==0) solved = true;
    }
}
