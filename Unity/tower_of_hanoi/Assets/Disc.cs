using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Disc : MonoBehaviour
{
    public static Disc get_disc_count;
    public TMP_InputField inputFiled;
    public string value;
    public void Awake()
    {
        if (get_disc_count == null)
        {
            get_disc_count = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void getDisc()
    {
        value = inputFiled.text;
        SceneManager.LoadSceneAsync(1);
    }

    // script 2
    /*public TextMeshProUGUI display_name;
        public void Awake()
        {
            display_name.text = getValue.get_value.name;
        }*/
}
