using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class StartGame : MonoBehaviour
{
    public GameObject cotPrefag;
    public GameObject dePrefag;
    public GameObject diaPrefag;

    public const int so_dia = 3;

    float start;
    float stop;

    List<GameObject> ds_dia = new List<GameObject>();

    private Vector3 pos_base_dia;
    private Vector3 scale_base_dia;
    // Start is called before the first frame update
    void Awake()
    {
        pos_base_dia = Vector3.zero;
        scale_base_dia = diaPrefag.transform.localScale;
        for (int i = 0; i < so_dia; i++)
        {
            ds_dia.Add(gameObject);
            Debug.Log("da them vao list");
        }

        for (int i = 0; i < so_dia; i++)
        {
            ds_dia[i] = Instantiate(diaPrefag);
            ds_dia[i].transform.position = new Vector3(0, 0, 0);
        }

        for (int i = 1; i < so_dia - 1; i++)
        {
            if (i > 0)
            {
                float range_dia_duoi = ds_dia[i - 1].transform.localScale.y;
                float range_dia_tren = ds_dia[i].transform.localScale.y;
                ds_dia[i].transform.position = new Vector3(0, range_dia_duoi + (range_dia_duoi * range_dia_tren * 2), 0);
            }
            else
            {
                ds_dia[i].transform.position = new Vector3(0, 0, 0);
            }
        }
    }

}
