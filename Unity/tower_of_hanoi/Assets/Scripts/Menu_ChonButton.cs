using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.AI;
using UnityEngine;

public class Menu_ChonButton : MonoBehaviour
{
    public GameObject menu_ChonButton;
    
    

    void Start()
    {
        menu_ChonButton = GameObject.FindGameObjectWithTag("ManHinh_ChonButton");
        Debug.Log(menu_ChonButton.name);
        menu_ChonButton.SetActive(false);
    }
    void Update()
    {
          
    }

    public void Done_Init()
    {
        Debug.Log("da click");
        GameInfo.done_init = true;
    }

    public void setActive(bool b)
    {
        menu_ChonButton.SetActive(b);
    }
}
