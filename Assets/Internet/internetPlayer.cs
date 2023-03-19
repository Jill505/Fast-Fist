using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class internetPlayer : NetworkBehaviour
{
    [SerializeField]
    public gameCore gameCores;

    [Networked]
    public int moodSelectionSort { get; set; }
    [Networked]
    public int characterSelectionSort { get; set; }

    private void Awake()
    {
        gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
    }
}
