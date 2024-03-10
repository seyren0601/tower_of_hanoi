using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Schema;
using tower_of_hanoi.Classes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.Mathf;

public class StartGame : MonoBehaviour
{
    public GameObject cotPrefag;
    public GameObject dePrefag;
    public GameObject diaPrefag;

    private Menu_ChonButton menu_ChonButton;

    public static int so_dia { get; set; }
    private const int so_cot = 3;

    public float thoigian_tha_dia;
    float speed_tha_dia;

    private const float heso_scale_x = 1.1f;
    private const float heso_scale_y = 1.054f;

    //private const float 
    public bool da_chon = false;
    int so_dia_hientai = 0;
    int index_dia_dang_chon;
    int index_cot_dang_chon;
    int cot_dang_click;

    int index_dia_tren_cung;

    Stack<GameObject> cot1 = new Stack<GameObject>();
    Stack<GameObject> cot2 = new Stack<GameObject>();
    Stack<GameObject> cot3 = new Stack<GameObject>();

    List<GameObject> ds_dia = new List<GameObject>();
    List<GameObject> ds_cot = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        menu_ChonButton = FindObjectOfType<Menu_ChonButton>();
        SpawnDia(); 
        SpawnCot();
        SpawnDe();
    }

    void Update()
    {
        if(!GameInfo.done_init)
        {
            Game_Init();
        }
        else if (GameInfo.done_init)
        {
            if (!GameInfo.May_play && !GameInfo.Player_play) menu_ChonButton.setActive(true);
            else if (GameInfo.Player_play) Player();
        }
    }

    void Player()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            Debug.Log(hit.collider.name);

            // neu click vao obj va click vao cot dau tien
            if (hit && Check_Name_RayHit(hit.collider.name) && !da_chon)
            {
                cot_dang_click = Find_Index_Cot(hit.collider.name);             
                float y = hit.collider.transform.position.y + (4f * Pow(heso_scale_y, so_dia));

                //neu click vao cot 1 thi lay index cua dia dc dua len va stack cot1 pop dia ra
                if (hit.collider.name == "Cot 1")
                {
                    index_dia_tren_cung = Find_Index_Dia(cot1.Peek().name);
                    cot1.Pop();
                }
                //neu click vao cot 2 thi lay index cua dia dc dua len va stack cot2 pop dia ra
                else if (hit.collider.name == "Cot 2")
                {
                    index_dia_tren_cung = Find_Index_Dia(cot2.Peek().name);
                    cot2.Pop();
                }
                //neu click vao cot 3 thi lay index cua dia dc dua len va stack cot3 pop dia ra
                else if (hit.collider.name == "Cot 3")
                {
                    index_dia_tren_cung = Find_Index_Dia(cot3.Peek().name);
                    cot3.Pop();
                }             

                //set trang thai cho dia dc chon thanh 0 co trong luc va di chuyen dia len
                ds_dia[index_dia_tren_cung].GetComponent<Rigidbody2D>().isKinematic = true;
                DiChuyen_Len(index_dia_tren_cung, y);
                da_chon = true;
            }

            // neu click vao obj va da click vao cot tiep theo
            else if (hit && Check_Name_RayHit(hit.collider.name) && da_chon)
            {
                float x, y;
                //lay index cot dang click vao tiep theo
                int index_cot_dang_click = Find_Index_Cot(hit.collider.name);
                if (hit.collider.name == "Cot 1")
                {
                    //lay dc vi tri x o giua cot 1
                    x = ds_cot[index_cot_dang_click].transform.position.x - 0.5f * Pow(heso_scale_y, so_dia);
                }
                else if (hit.collider.name == "Cot 2")
                {
                    //lay dc vi tri x o giua cot 2
                    x = ds_cot[index_cot_dang_click].transform.position.x - 0.5f * Pow(heso_scale_y, so_dia);
                }
                else
                {
                    //lay dc vi tri x o giua cot 3
                    x = ds_cot[index_cot_dang_click].transform.position.x - 0.5f * Pow(heso_scale_y, so_dia);
                }

                //kiem tra xem cot dang click co rong ko
                if ((hit.collider.name == "Cot 1" ? cot1.Count : hit.collider.name == "Cot 2" ? cot2.Count : cot3.Count) != 0) //cot khong rong  
                {
                    //Nếu tên của collider được click là "Cot 1-2-3", thì stackPeekName sẽ là tên của đỉnh của cot1-2-3. 
                    string stackPeekName = hit.collider.name == "Cot 1" ? cot1.Peek().name : hit.collider.name == "Cot 2" ? cot2.Peek().name : cot3.Peek().name;
                    if (int.TryParse(stackPeekName, out int stackPeekValue) && int.TryParse(ds_dia[index_dia_tren_cung].name, out int dsCotValue))
                    {
                        //kiểm tra nếu  đĩa ở cột đang chọn > đĩa đã đưa lên ở trên thì cho phép di chuyển
                        if (stackPeekValue > dsCotValue)                        
                        {
                            DiChuyen_TraiPhai(index_dia_tren_cung, x);
                            //di chuyển xong thì cột sẽ push đĩa dc thả xuống vào stack
                            (hit.collider.name == "Cot 1" ? cot1 : hit.collider.name == "Cot 2" ? cot2 : cot3).Push(ds_dia[index_dia_tren_cung]);
                            ds_dia[index_dia_tren_cung].GetComponent<Rigidbody2D>().isKinematic = false;
                        }
                        else // nguoc lai k cho phep di chuyen 
                        {
                            ds_dia[index_dia_tren_cung].GetComponent<Rigidbody2D>().isKinematic = false;
                            //push vao lai vi tri cu
                            (ds_cot[cot_dang_click].name == "Cot 1" ? cot1 : ds_cot[cot_dang_click].name == "Cot 2" ? cot2 : cot3).Push(ds_dia[index_dia_tren_cung]);

                        }
                    }                    
                }
                else // nguoc lai cot rong stack empty thi k can so sanh dia
                {
                    DiChuyen_TraiPhai(index_dia_tren_cung, x);
                    (hit.collider.name == "Cot 1" ? cot1 : hit.collider.name == "Cot 2" ? cot2 : cot3).Push(ds_dia[index_dia_tren_cung]);
                    ds_dia[index_dia_tren_cung].GetComponent<Rigidbody2D>().isKinematic = false;
                }
                da_chon = false;
                index_dia_tren_cung = -1;
            }
        }
        if (cot3.Count == so_dia)
        {
            //load screen Player_Won sau 0,75s
            StartCoroutine(LoadSceneAfterDelay(3, 0.75f));
        }
    }
    //
    IEnumerator LoadSceneAfterDelay(int sceneIndex, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(sceneIndex);
    }

    void DiChuyen_TraiPhai(int index, float x)
    {
        //di chuyen sang trai
        if (ds_dia[index].transform.position.x > x)
        {
            while (ds_dia[index].transform.position.x > x)
            {
                ds_dia[index].transform.position += new Vector3(-1, 0);

            }
            // neu vi tri cua dia di chuyen qua vi tri giua cot thi set lai x = vitri giua cot 
            if (ds_dia[index].transform.position.x < x)
            {
                ds_dia[index].transform.position = new Vector3(x, 0);
            }
        }
        //di chuyen sang phai
        else if (ds_dia[index].transform.position.x < x)
        {
            while (ds_dia[index].transform.position.x < x)
            {
                ds_dia[index].transform.position += new Vector3(1, 0);
            }
            if (ds_dia[index].transform.position.x > x)
            {
                ds_dia[index].transform.position = new Vector3(x, 0);
            }
        }
    }
    void DiChuyen_Len(int index, float y)
    {
        if (ds_dia[index].transform.position.y < y)
        {
            while (ds_dia[index].transform.position.y < y)
            {
                ds_dia[index].transform.position += new Vector3(0, 1);
            }
            if (ds_dia[index].transform.position.y > y)
            {
                ds_dia[index].transform.position = new Vector3(ds_dia[index].transform.position.x, y);
            }
        }
    }

    void Game_Init()
    {
        speed_tha_dia -= Time.deltaTime;
        if (speed_tha_dia <= 0)
        {
            if (so_dia_hientai < so_dia)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Da Click");

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                    Debug.Log(hit.collider.name);

                    if (hit && Check_Name_RayHit(hit.collider.name) && so_dia_hientai < so_dia)
                    {
                        int cot_current = Find_Index_Cot(hit.collider.name);

                        ds_dia[so_dia_hientai].transform.position = new Vector3(0, 0, 0);
                        ds_dia[so_dia_hientai].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        ds_dia[so_dia_hientai].transform.position = new Vector3(
                            ds_cot[cot_current].transform.position.x - 0.5f * Pow(heso_scale_y, so_dia),
                            ds_cot[cot_current].transform.position.y + (4f * Pow(heso_scale_y, so_dia)),
                            ds_cot[cot_current].transform.position.z);


                        if (hit.collider.name == "Cot 1")
                        {
                            cot1.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot1.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot1_int.Push(so_dia - so_dia_hientai);
                            Debug.Log(GameInfo.cot1.Peek().name);
                        }
                        else if (hit.collider.name == "Cot 2")
                        {
                            cot2.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot2.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot2_int.Push(so_dia - so_dia_hientai);
                        }
                        else if (hit.collider.name == "Cot 3")
                        {
                            cot3.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot3.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot3_int.Push(so_dia - so_dia_hientai);
                        }
                        so_dia_hientai++;
                        speed_tha_dia = thoigian_tha_dia;
                    }
                }
            }
            else
            {
                GameInfo.done_init = true;
            }
        }
    }

    void SpawnDia()
    {
        float base_dia = diaPrefag.transform.localScale.x;


        ds_dia = new List<GameObject>(so_dia);

        GameObject dia = new GameObject();
        for (int i = so_dia - 1; i >= 0; i--)
        {
            dia = Instantiate(diaPrefag);
            dia.transform.localScale = new Vector2(base_dia * Pow(heso_scale_x, i + 1), base_dia * Pow(heso_scale_y, i + 1));
            dia.name = "" + (i + 1);
            ds_dia.Add(dia);
            Debug.Log("da them vao list");
        }
        //so_dia_hientai = so_dia - 1;
    }

    void SpawnCot()
    {
        float range = (1.45f * Pow(1.1f, so_dia) * 2) + 3f;
        float scale_x = cotPrefag.transform.localScale.x * Pow(1.054f, so_dia);
        float scale_y = cotPrefag.transform.localScale.y * Pow(1.1f, so_dia);
        float scale_z = cotPrefag.transform.localScale.z;

        float pos_y_de = dePrefag.transform.position.y + 0.55f;

        float value = 0;
        float temp = 5.05f;

        for (int i = 1; i <= so_dia; i++)
        {
            value += (temp * 0.1f) / 2;
            temp *= 1.1f;
        }

        for (int i = 0; i < so_cot; i++)
        {
            ds_cot.Add(cotPrefag);
            Debug.Log("da them cot vao list");

            ds_cot[i] = Instantiate(cotPrefag);
            ds_cot[i].name = $"Cot {i + 1}";
            ds_cot[i].transform.localScale = new Vector3(scale_x, scale_y, scale_z);

            if (i == 0) ds_cot[i].transform.position = new Vector3(-range + 0.5f, (2.65f + pos_y_de) + value, 0);
            else ds_cot[i].transform.position = new Vector3(ds_cot[i - 1].transform.position.x + range, (2.65f + pos_y_de) + value, 0);
        }
        GameInfo.toado_cot1 = new Vector2(ds_cot[0].transform.position.x - 0.5f * Pow(heso_scale_y, so_dia), ds_cot[0].transform.position.y + (4f * Pow(heso_scale_y, so_dia)));
        GameInfo.toado_cot2 = new Vector2(ds_cot[1].transform.position.x - 0.5f * Pow(heso_scale_y, so_dia), ds_cot[1].transform.position.y + (4f * Pow(heso_scale_y, so_dia)));
        GameInfo.toado_cot3 = new Vector2(ds_cot[2].transform.position.x - 0.5f * Pow(heso_scale_y, so_dia), ds_cot[2].transform.position.y + (4f * Pow(heso_scale_y, so_dia)));
    }

    void SpawnDe()
    {
        float scale_x = dePrefag.transform.localScale.x * Pow(1.1f, so_dia);
        float scale_y = dePrefag.transform.localScale.y;
        float scale_z = dePrefag.transform.localScale.z;

        dePrefag.transform.localScale = new Vector3(scale_x, scale_y, scale_z);
    }

    bool Check_Name_RayHit(string name)
    {
        for (int i = 0; i < ds_cot.Count; i++)
        {
            if (name == ds_cot[i].name) return true;
        }
        return false;
    }

    int Find_Index_Cot(string name)
    {
        for (int i = 0; i < ds_cot.Count; i++)
        {
            if (name == ds_cot[i].name) return i;
        }
        return -1;
    }

    int Find_Index_Dia(string name)
    {
        for (int i = 0; i < ds_dia.Count; i++)
        {
            if (name == ds_dia[i].name) return i;
        }
        return -1;
    }
}
