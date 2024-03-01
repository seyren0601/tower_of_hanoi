using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class StartGame : MonoBehaviour
{
    public GameObject cotPrefag;
    public GameObject dePrefag;
    public GameObject diaPrefag;

    public const int so_dia = 3;
    int so_dia_hientai = 0;

    public float start;
    public float stop;

    List<GameObject> ds_dia = new List<GameObject>();

    private InputHandler inputHandler;
    InputAction.CallbackContext context;

    private Vector3 pos_base_dia;
    private Vector3 scale_base_dia;
    // Start is called before the first frame update
    void Start()
    {
        pos_base_dia = Vector3.zero;
        scale_base_dia = diaPrefag.transform.localScale;
        SpawnDia();

    }

    void Update()
    {
        if (so_dia_hientai < so_dia)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Da Click");
                Debug.Log(Input.mousePosition);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider != null && hit != null && hit.collider.name == cotPrefag.name)
                {
                    Debug.Log(hit.collider.name);
                    ds_dia[so_dia_hientai] = Instantiate(diaPrefag);
                    ds_dia[so_dia_hientai].transform.position = new Vector3(0, 0, 0);
                    ds_dia[so_dia_hientai].transform.position = new Vector3(cotPrefag.transform.position.x - 0.5f, cotPrefag.transform.position.y + 4f, cotPrefag.transform.position.z);
                    so_dia_hientai++;
                }
                

            }
        }
    }



    void SpawnDia()
    { 
        for (int i = 0; i < so_dia; i++)
        {
            ds_dia.Add(gameObject);
            Debug.Log("da them vao list");
        }

    }
}
