using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class internetPlayer : NetworkBehaviour
{
    [SerializeField]
    public gameCore gameCores;

    public int myPlayerSort;

   

    [Networked]
    public int moodSelectionSort { get; set; }
    [Networked]
    public int characterSelectionSort { get; set; }


    //���a��¦�ƾ�
    [Networked]
    public float Hps { get; set; }
    [Networked]
    public float Str { get; set; }
    [Networked]
    public float Rac { get; set; }
    [Networked]
    public float Cur { get; set; }
    [Networked]
    public float Ablock { get; set; }

    //���a

    private void Awake()
    {
        gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
    }


    public override void FixedUpdateNetwork()
    {
        //�P�B�����ƾ�
    }

    public void CharacterGivingValue(int characterSorting)
    {
        characterSelectionSort = characterSorting;

        if (characterSorting == 0)
        {
            //Defult Character
            Hps = 60f;
            Str = 30f;
            Rac = 90f;
            Cur = 90f;
            Ablock = 0.2f;
        }
        else if (characterSorting == 1)
        {
            //���L
        }
        else if (characterSorting == 2)
        {
            //�´�
        }
    }

}


