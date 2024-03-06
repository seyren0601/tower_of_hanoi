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
        menu_ChonButton.SetActive(true);
        GameInfo.done_init = true;
    }

    public void Player_Play()
    {
        Debug.Log("Chọn cho Player chơi");
        menu_ChonButton.SetActive(false);
        GameInfo.Player_play = true;
    }

    public void May_Play() 
    {
        Debug.Log("Chọn cho Máy chơi");
        menu_ChonButton.SetActive(false);
        GameInfo.May_play = true;
    }

    public void setActive(bool b)
    {
        menu_ChonButton.SetActive(b);
    }
}
