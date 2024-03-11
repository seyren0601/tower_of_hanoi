using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private bool audioBegin = false;
    private GameObject audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GameObject.FindGameObjectWithTag("Main menu theme");
        if(audio == this.gameObject){
            if(!audioBegin){
                DontDestroyOnLoad(this.gameObject);
                audioBegin = true;
            }
        }
        else{
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
