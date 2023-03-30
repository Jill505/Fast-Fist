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

    public GameObject atkDirFistHint;//之後斟酌刪

   [Networked]
   public string playerName { get; set; }

    [Networked]
    public int moodSelectionSort { get; set; }
    [Networked]
    public int characterSelectionSort { get; set; }


    //玩家基礎數據
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


    //現存玩家數據
    [Networked] public float AtkTime {get;set; }
    [Networked] public float Energy { get; set; }
    [Networked] public float UltComsume { get; set; }
    [Networked] public float MaxmentEnergy { get; set; }


    //玩家

    //測試
    [Networked]
    public int TestVariable { get; set; }

    //網路呼叫
    [Networked] public bool AtkFinishedCall { get; set; }
    [Networked] public bool DefFinishedCall { get; set; }

    //網路傳輸數據-交換用
    [Networked] public float AtkStr { get; set; }
    [Networked] public float AtkDir { get; set; }
    [Networked] public float DefDir { get; set; }
    [Networked] public float FrameChanged { get; set; }
    [Networked] public float DefCur { get; set; }
    [Networked] public float DefAblock { get; set; }

    //本地呼叫
    public bool AtkCallTired;
    public bool DefCallTired;

    //float delLaterJustMakeSure;

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
        //if (Object.HasInputAuthority)//是該玩家操控
        //{
        if (Object.HasInputAuthority)
        {
            Runner.gameObject.GetComponent<gameIinker>().myPlayer = this;
            myGamelinker = Runner.gameObject.GetComponent<gameIinker>();
        }
        if (gameCores.numberIntheScene == 2)
        {
            myPlayerSort = 1;
            Debug.Log("透過抑制器解決了問題");
        }
        //dirInput.gameRunner = myGamelinker;
        //}
        Debug.Log("Start執行了一次喔~");
    }

    public PlayerRef myLocalPlayer()
    {
        return Runner.LocalPlayer;
    }

    public override void FixedUpdateNetwork()
    {

        //playerName = localDataBase.PlayerData.Name;
        gameCores.Rpc_namePlayer();


        //同步網路數據
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
            //playerName = localDataBase.PlayerData.Name;

            CharacterGivingValue(characterSelectionSort);

            Runner.gameObject.GetComponent<gameIinker>().brocastedName = true;
        }

        if (GetInput(out AtkInputData data1))
        {
            AtkDir = data1.AtkDir;
            AtkFinishedCall = data1.AtkFinished;
            AtkStr = data1.AtkStr;
            Debug.Log("接收到攻擊資料");
        }

        if (GetInput(out DefInputData data2))
        {
            DefDir = data2.DefDir;
            DefFinishedCall = data2.DefFinished;
            Debug.Log("接收到防守資");
            //data2.DefCur = DefCur;
            //data2.ABloack = DefAblock;
        }

        if (GetInput(out PlayerInformation data5))
        {
            Hps = data5.Hps;
            Str = data5.Str;
            Rac = data5.Rac;
            Cur = data5.Cur;
            Ablock = data5.Ablock;

            MaxmentEnergy = data5.MaxmentEnergy;
            Energy = data5.Energy;
        }


        if (Object.HasInputAuthority)//玩家有該角色操控權限
        {
            if (gameCores.startGameBool)//遊戲開始
            {
                if (myPlayerSort == 0)//玩家序列為0
                {
                    if (gameCores.turnToPlayer0 == true)//輪到自己回合
                    {
                        if (gameCores.AttackCall == true)//是 提供攻擊
                        {
                            if (!AtkCallTired)
                            {
                                //只執行一次
                                AtkCallTired = true;
                                StartCoroutine(Atk());
                            }
                            //完成攻擊偵測
                            DefCallTired = false;
                        }
                    }
                    else
                    {
                        if (gameCores.DefendCall == true)//否 提供防守
                        {
                            if (!DefCallTired)
                            {
                                //只執行一次
                                DefCallTired = true;
                                StartCoroutine(Def());
                            }
                        }
                        AtkCallTired = false;
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
                                //只執行一次
                                AtkCallTired = true;
                                StartCoroutine(Atk());
                            }
                            DefCallTired = false;
                        }
                    }
                    else
                    {
                        if (gameCores.DefendCall == true)
                        {
                            if (!DefCallTired)
                            {
                                //只執行一次
                                DefCallTired = true;
                                StartCoroutine(Def());
                            }
                            AtkCallTired = false;
                        }
                    }
                }
            }
        }

        //delLaterJustMakeSure++;
        //atkDirFistHint.transform.rotation = Quaternion.Euler(0f, 0f, delLaterJustMakeSure);
        atkDirFistHint.transform.rotation = Quaternion.Euler(0f, 0f, gameCores.SwapAtkDir);
    }

    public void CharacterGivingValue(int characterSorting)
    {
        playerName = localDataBase.PlayerData.Name;
        gameCores.Rpc_namePlayer();


        characterSelectionSort = characterSorting;

        if (characterSorting == 0)
        {
            //Defult Character
            Hps = 100f;
            Str = 10f;
            Rac = 300f;
            Cur = 120f;
            Ablock = 0.2f;

            AtkTime = 5f;

            UltComsume = 150f;
            MaxmentEnergy = 150f;
        }
        else if (characterSorting == 1)
        {
            //蝦兵
        }
        else if (characterSorting == 2)
        {
            //黑斯
        }
    }


    IEnumerator Atk()
    {
        dirInput.allowInputAtk = true;
        //Invoke("AtkBreak",AtkTime);
        yield return null;
        //顯示攻擊提示圓圈
        attackHintCircle.showMe();
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("輪到你進行攻擊了！");
    }
    void AtkBreak()
    {
        dirInput.allowInputAtk = false;
    }

    IEnumerator Def()
    {
        Invoke("DefBreak", Rac/100);
        yield return null;
        dirInput.allowInputDef = true;
        //顯示防守動畫
        //atkDirFistHint.transform.rotation = Quaternion.Euler(0f, 0f, gameCores.SwapAtkDir-90f);
        atkDirFistHint.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 255f);
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("攻擊來了 快格檔！");
        yield return null;
    }
    void DefBreak()
    {
        DefDir = -1000f;

        CancelInvoke("DefBreak");

        myGamelinker.DefCall = true;
        dirInput.allowInputDef = false;

        atkDirFistHint.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
    }

    public void AtkDataGiving(float attackDiraction)
    {
        AtkDir = attackDiraction;
        //發送訊息
        CancelInvoke("AtkBreak");
        //圈圈消失 完成Atk()剩下該做的事情
        attackHintCircle.hideMe();

        myGamelinker.AtkCall = true;//呼叫gamelinker上傳
        dirInput.allowInputAtk = false;
    }

    public void DefDataGiving(float defendDiraction)
    {
        DefDir = defendDiraction;

        CancelInvoke("DefBreak");

        myGamelinker.DefCall = true;
        dirInput.allowInputDef = false;

        atkDirFistHint.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
    }
}


