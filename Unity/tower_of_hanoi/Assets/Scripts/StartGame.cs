using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

    public static int so_dia { get; set; } = 5;
    private const int so_cot = 3;

    public float thoigian_tha_dia;
    float speed_tha_dia;

    private const float heso_scale_x = 1.1f;
    private const float heso_scale_y = 1.054f;
    //private const float 

    int so_dia_hientai = 0;

    Stack<GameObject> cot1 = new Stack<GameObject>();
    Stack<GameObject> cot2 = new Stack<GameObject>();
    Stack<GameObject> cot3 = new Stack<GameObject>();

    List<GameObject> ds_dia = new List<GameObject>();
    List<GameObject> ds_cot = new List<GameObject>();

    List<Canvas> text = new List<Canvas>();

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
            if (!GameInfo.May_play) menu_ChonButton.setActive(true);
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
                            GameInfo.cot1_int.Push(so_dia -  so_dia_hientai);
                            Debug.Log(GameInfo.cot1.Peek().name);
                        }
                        else if (hit.collider.name == "Cot 2")
                        {
                            cot2.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot2.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot2_int.Push(so_dia -  so_dia_hientai);
                        }
                        else if (hit.collider.name == "Cot 3")
                        {
                            cot3.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot3.Push(ds_dia[so_dia_hientai]);
                            GameInfo.cot3_int.Push(so_dia -  so_dia_hientai);
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

        for (int i = 0;i < so_cot;i++) 
        {
            ds_cot.Add(cotPrefag);
            Debug.Log("da them cot vao list");

            ds_cot[i] = Instantiate(cotPrefag);
            ds_cot[i].name = $"Cot {i+1}";
            ds_cot[i].transform.localScale = new Vector3(scale_x , scale_y, scale_z);

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
}
