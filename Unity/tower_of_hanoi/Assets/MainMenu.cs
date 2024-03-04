using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void btn_ClickToPlay()
    {
        SceneManager.LoadScene(1);
    }
    public void btn_Continue()
    {

    }
    public void btn_QuitGame()
    {
        Application.Quit();
    }
    public void btn_BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
