using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static float disc_count { get; set;}
    public static bool done_init { get; set; }
    public static bool May_play {  get; set; }

    public static Stack<GameObject> cot1 { get; set; } = new Stack<GameObject>();
    public static Stack<GameObject> cot2 { get; set; } = new Stack<GameObject>();
    public static Stack<GameObject> cot3 { get; set; } = new Stack<GameObject>();

    public static Vector2 toado_cot1 { get; set; }
    public static Vector2 toado_cot2 { get; set; }
    public static Vector2 toado_cot3 { get; set; }


    // Start is called before the first frame update
    void Awake()
    {
        disc_count = StartGame.so_dia;
        done_init = false;
        Player_play = false;
        May_play = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
