using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmController : MonoBehaviour
{
    static public bool isMute = false;

    public string songName;
    private void Awake()
    {
         if (songName != null)
        {
            //§ó§ïÀÉ®×
        }
    }
    void Start()
    {
        if (isMute == true)
        {
            gameObject.GetComponent<AudioSource>().volume = 0f;
        }
    }

    void Update()
    {
        
    }
}
