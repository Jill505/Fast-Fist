using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class internetPlayer : NetworkBehaviour
{
    [SerializeField]
    public gameCore gameCores;

    public gameIinker myGamelinker;

    public int myPlayerSort;

    public DirInput dirInput;

   [Networked]
   public string playerName { get; set; }

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


    //�{�s���a�ƾ�
    [Networked] public float AtkTime {get;set; }
    [Networked] public float Energy { get; set; }
    [Networked] public float UltComsume { get; set; }
    [Networked] public float MaxmentEnergy { get; set; }


    //���a
    [Networked] public float AtkDir { get; set; }
    [Networked] public float DefDir { get; set; }
    [Networked] public float FrameChanged { get; set; }

    //����
    [Networked]
    public int TestVariable { get; set; }

    //���a
    public bool AtkCallTired;
    public bool DefCallTired;

    private void Awake()
    {
        gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
        dirInput = GameObject.Find("DirInputSystem").GetComponent<DirInput>();

        if (Object.HasInputAuthority)//�O�Ӫ��a�ޱ�
        {
            Runner.gameObject.GetComponent<gameIinker>().myPlayer = this;
            myGamelinker = Runner.gameObject.GetComponent<gameIinker>();
        }
        //playerName = localDataBase.PlayerData.Name;
    }

    public PlayerRef myLocalPlayer()
    {
        return Runner.LocalPlayer;
    }

    public override void FixedUpdateNetwork()
    {

        //playerName = localDataBase.PlayerData.Name;
        gameCores.Rpc_namePlayer();


        //�P�B�����ƾ�
        bool swapBool;
        swapBool = gameCores.turnToPlayer0;
        swapBool = gameCores.AttackCall;
        swapBool = gameCores.DefendCall;

        if (GetInput(out TestStruck data))
        {
            TestVariable += data.uploadInt;
            if (myPlayerSort != 0)
            {
                TestVariable -= 2 * data.uploadInt;
            }
        }

        if (GetInput(out playerInputData data0))
        {
            characterSelectionSort = data0.characterSelectionSort;
            moodSelectionSort = data0.moodSelectionSort;
            playerName = localDataBase.PlayerData.Name;

            CharacterGivingValue(characterSelectionSort);

            myPlayerSort = gameCores.numberIntheScene;

            Runner.gameObject.GetComponent<gameIinker>().brocastedName = true;
        }

        if (GetInput(out AtkInputData data1))
        {
            data1.AtkDir = AtkDir;
        }


        if (Object.HasInputAuthority)//���a���Ө���ޱ��v��
        {
            if (gameCores.startGameBool)//�C���}�l
            {
                if (myPlayerSort == 0)//���a�ǦC��0
                {
                    if (gameCores.turnToPlayer0 == true)//����ۤv�^�X
                    {
                        if (gameCores.AttackCall == true)//�O ���ѧ���
                        {
                            if (!AtkCallTired)
                            {
                                //�u����@��
                                AtkCallTired = true;
                                StartCoroutine(Atk());
                            }
                            //������������

                        }
                    }
                    else
                    {
                        if (gameCores.DefendCall == true)//�_ ���Ѩ��u
                        {
                            if (!DefCallTired)
                            {
                                //�u����@��
                                DefCallTired = true;
                                StartCoroutine(Def());
                            }
                        }
                    }
                }

                else if (myPlayerSort == 1)
                {
                    if (gameCores.turnToPlayer0 == false)
                    {
                        if (gameCores.AttackCall == true)
                        {
                            if (!AtkCallTired)
                            {
                                //�u����@��
                                AtkCallTired = true;
                                StartCoroutine(Atk());
                            }
                        }
                    }
                    else
                    {
                        if (gameCores.DefendCall == true)
                        {
                            if (!DefCallTired)
                            {
                                //�u����@��
                                DefCallTired = true;
                                StartCoroutine(Def());
                            }
                        }
                    }
                }

            }
        }


    }

    public void CharacterGivingValue(int characterSorting)
    {
        playerName = localDataBase.PlayerData.Name;
        gameCores.Rpc_namePlayer();


        characterSelectionSort = characterSorting;

        if (characterSorting == 0)
        {
            //Defult Character
            Hps = 60f;
            Str = 30f;
            Rac = 90f;
            Cur = 90f;
            Ablock = 0.2f;

            AtkTime = 1f;

            UltComsume = 150f;
            MaxmentEnergy = 150f;
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


    IEnumerator Atk()
    {
        dirInput.allowInputAtk = true;
        Invoke("AtkBreak",AtkTime);
        yield return null;
        //��ܧ������ܶ��
        
    }
    void AtkBreak()
    {
        dirInput.allowInputAtk = false;
    }

    IEnumerator Def()
    {
        yield return null;
        //��ܨ��u�ʵe
    }
    void DefBreak()
    {

    }

    public void AtkDataGiving(float attackDiraction)
    {
        AtkDir = attackDiraction;
        //�o�e�T��
        CancelInvoke("AtkBreak");
        //������ ����Atk()�ѤU�Ӱ����Ʊ�
        myGamelinker.AtkCall = true;
    }
}


