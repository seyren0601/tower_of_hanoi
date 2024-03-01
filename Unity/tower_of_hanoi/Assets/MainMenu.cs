using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ClickToPlay()
    {
        SceneManager.LoadScene(1);
    }
    public void Continue()
    {

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
