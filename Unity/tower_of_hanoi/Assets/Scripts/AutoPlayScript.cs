using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using tower_of_hanoi.Classes;
using UnityEngine;

public class AutoPlayScript:MonoBehaviour
{
    bool solved = false;
    Vector2 cot1;
    Vector2 cot2;
    Vector3 cot3;
    Stack<GameObject> stack_cot1;
    Stack<GameObject> stack_cot2;
    Stack<GameObject> stack_cot3;
    float disc_count;

    bool algorithm_init = false;
    Stack<GameObject>[] cot_list = new Stack<GameObject>[3];
    List<(int, int)> Moves = new List<(int, int)>();
    
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    bool handled = false;
    void Update()
    {
        if (GameInfo.May_play && !algorithm_init)
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
            Algorithm.Solve_Recursion((int)disc_count, 0, 2, 1);
            Moves = Algorithm.Moves;
            Debug.Log(Moves.Count);
            algorithm_init=true;
        }
        if(!solved && algorithm_init && !handled){
            StartCoroutine(AutoSolve());
            if(cot_list[2].Count == disc_count) solved=true;
        }
    }

    void MoveDisc(int from, int to)
    {
        GameObject obj_from = cot_list[from].Pop();
        cot_list[to].Push(obj_from);
        obj_from.transform.position = (to == 0) ? cot1 : (to == 1) ? cot2 : cot3;
    }

    IEnumerator AutoSolve()
    {
        handled = true;
        for (int i = 0; i < Moves.Count; i++)
        {
            (int, int) move = Moves[i];
            MoveDisc(move.Item1, move.Item2);
            yield return new WaitForSecondsRealtime(1);
        }
        solved = true;
    }

    void NextMove(){
        if(!solved){
            MoveDisc(Moves[0].Item1, Moves[0].Item2);
        }
        if(Moves.Count==0) solved = true;
    }
}