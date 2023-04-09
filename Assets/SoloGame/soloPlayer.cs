using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soloPlayer : MonoBehaviour
{
    soloCenter center;
    dirInputSolo dirinput;


    //���a��¦�ƾ�
    public float Hps;
    public float Str;
    public float Rac;
    public float Cur;
    public float Ablock;


    //�{�s���a�ƾ�
    public float AtkTime;
    public float Energy;
    public float UltComsume;
    public float MaxmentEnergy;

    private void Awake()
    {
        center = GameObject.Find("SoloCenter").GetComponent<soloCenter>();
        center.player = this;
        dirinput = GameObject.Find("DirInput").GetComponent<dirInputSolo>();

        //���
        sortingPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (center.gameStarted)//�C���}�l
        {
            /*
            if (center.turnToPlayer == true)//����ۤv�^�X
            {
                //����
                dirinput.allowInputAtk = true;
                //��ܧ�����
            }
            else
            {
                //���u
                dirinput.allowInputDef = true;
            }*/
        }
    }

    void sortingPlayer()
    {
        if (localDataBase.PlayerData.selectionCharacter == 0)
        {
            Hps = 60f;
            Str = 30f;
            Rac = 100f;
            Cur = 90f;
            Ablock = 0.1f;

            AtkTime = 5f;

            UltComsume = 150f;
            MaxmentEnergy = 150f;
        }
    }
}
