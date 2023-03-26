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
    public circleRounding attackHintCircle;

    public GameObject atkDirFistHint;//����r�u�R

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

    //����
    [Networked]
    public int TestVariable { get; set; }

    //�����I�s
    [Networked] public bool AtkFinishedCall { get; set; }
    [Networked] public bool DefFinishedCall { get; set; }

    //�����ǿ�ƾ�-�洫��
    [Networked] public float AtkStr { get; set; }
    [Networked] public float AtkDir { get; set; }
    [Networked] public float DefDir { get; set; }
    [Networked] public float FrameChanged { get; set; }
    [Networked] public float DefCur { get; set; }
    [Networked] public float DefAblock { get; set; }

    //���a�I�s
    public bool AtkCallTired;
    public bool DefCallTired;

    private void Awake()
    {
        gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
        dirInput = GameObject.Find("DirInputSystem").GetComponent<DirInput>();
        dirInput.myInternetPlayer = this;
        attackHintCircle = dirInput.gameObject.transform.GetChild(0).GetComponent<circleRounding>();
        atkDirFistHint = GameObject.Find("fist_Defult");

        //playerName = localDataBase.PlayerData.Name;
    }

    private void Start()
    {
        //if (Object.HasInputAuthority)//�O�Ӫ��a�ޱ�
        //{
            Runner.gameObject.GetComponent<gameIinker>().myPlayer = this;
            myGamelinker = Runner.gameObject.GetComponent<gameIinker>();
        //dirInput.gameRunner = myGamelinker;
        //}
        Debug.Log("Start����F�@����~");
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
            data1.AtkFinished = AtkFinishedCall;
            data1.AtkStr = AtkStr;
        }

        if (GetInput(out DefInputData data2))
        {
            data2.DefDir = DefDir;
            data2.DefFinished = DefFinishedCall;
            //data2.DefCur = DefCur;
            //data2.ABloack = DefAblock;
        }

        if (GetInput(out PlayerInformation data5))
        {
            data5.Hps = Hps;
            data5.Str = Str;
            data5.Rac = Rac;
            data5.Cur = Cur;
            data5.Ablock = Ablock;

            data5.MaxmentEnergy = MaxmentEnergy;
            data5.Energy = Energy;
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
        //Invoke("AtkBreak",AtkTime);
        yield return null;
        //��ܧ������ܶ��
        attackHintCircle.showMe();
    }
    void AtkBreak()
    {
        dirInput.allowInputAtk = false;
    }

    IEnumerator Def()
    {
        yield return null;
        dirInput.allowInputDef = true;
        //��ܨ��u�ʵe
        atkDirFistHint.transform.rotation = Quaternion.Euler(0f, 0f, gameCores.SwapAtkDir);
    }
    void DefBreak()
    {
        dirInput.allowInputDef = false;
    }

    public void AtkDataGiving(float attackDiraction)
    {
        AtkDir = attackDiraction;
        //�o�e�T��
        CancelInvoke("AtkBreak");
        //������ ����Atk()�ѤU�Ӱ����Ʊ�
        attackHintCircle.hideMe();

        myGamelinker.AtkCall = true;//�I�sgamelinker�W��
        dirInput.allowInputAtk = false;
    }

    public void DefDataGiving(float defendDiraction)
    {
        DefDir = defendDiraction;
        CancelInvoke("DefBreak");
        myGamelinker.DefCall = true;
        dirInput.allowInputDef = false;
    }
}


