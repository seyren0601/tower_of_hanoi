using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Xml.Schema;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject cotPrefag;
    public GameObject dePrefag;
    public GameObject diaPrefag;

    public const int so_cot = 3;

    private GameObject dia1;
    private GameObject dia2;
    private GameObject dia3;

    // Start is called before the first frame update
    void Awake()
    {
        dia1 = Instantiate(diaPrefag);
        dia2 = Instantiate(diaPrefag);
        dia3 = Instantiate(diaPrefag);
        dia1.transform.position = Vector3.zero;
        dia2.transform.position = new Vector3(dia1.transform.position.x * 1.2f, 0, 0);
        dia3.transform.position = new Vector3(dia1.transform.position.x * (1.2f*1.2f), 0, 0);
    }

}
