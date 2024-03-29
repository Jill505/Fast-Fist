using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class gameCore : NetworkBehaviour
{
    [Networked] public int numberIntheScene { get; set; }

    [Networked] public string p0NameString { get; set; }
    [Networked] public string p1NameString { get; set; }

    public internetPlayer p0;
    public internetPlayer p1;

    //共用數據
    [Networked(OnChanged = nameof(changeFrame))] public float Frame { get; set; }//架式
    [Networked] public float nextRoundCoolDown { get; set; }

    [Networked] public float SwapAtkStr { get; set; }
    [Networked] public float SwapAtkDir { get; set; }
    [Networked] public float SwapDefDir { get; set; }
    [Networked] public float SwapABloack { get; set; }
    [Networked] public float SwapCur { get; set; }

    [Networked] public int SwapDamageType { get; set; }//0=沒有 1=部分格檔 2=精準格檔

    [Networked] public float SwapFrameChange { get; set; }

    [Networked] public float SwapEnergyRecover { get; set; }

    [Networked] public int roundPassed { get; set; }

    //回合歸屬(誰為攻方)
    [Networked] public bool turnToPlayer0{ get; set; }

    //我自己取的名字! "網路板機"
    //狀態機一律以player1 為true時觸發 player2相反
    [Networked] public bool AttackCall { get; set; }
    [Networked] public bool DefendCall { get; set; }

    //開始呼叫
    [Networked] public bool startGameBool { get; set; }

    //網路呼叫
    [Networked] public bool endGameBool { get; set; }

    //本地數據 
    public AudioSource bgmPlayer;
    public AudioSource effectPlayer;
    public circleRounding AttackAllowCircle;
    public bool gameEndBlock;

        //Centrol控制
    public GameObject UIpos;
    public GameObject blackFilter;
    public GameObject killLIne0;
    public GameObject killLine1;
    public GameObject FramePos;

    public Text swapFrameShow;

    //動畫器(local)



    public Text player0Name;
    public Text player1Name;

    private void Start()
    {
        endGameBool = false;
        gameEndBlock = false;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_namePlayer()
    {
        if (p0 != null)
        {
             player0Name.text = "P0";
            //player0Name.text = p0.playerName;
        }
        if (p1 != null)
        {
            player1Name.text = "P1";
            //player0Name.text = p1.playerName;
        }
    }

    public static void changeFrame(Changed<gameCore> changed)
    {
        changed.Behaviour.swapFrameShow.text = "架式："+ changed.Behaviour.Frame;
    }

    public void reStartGame()
    {
        Frame = 0f;
        StartCoroutine(startGameIEnumerator());
    }

    IEnumerator startGameIEnumerator()
    {
        yield return new WaitForSeconds(3f);
        //數據同步
        nextRoundCoolDown = 0.6f;
        //playMusic
        bgmPlayer.clip = Resources.Load<AudioClip>("combatFile/FastFist_Fighting_common");//Capture BGM
        bgmPlayer.Play();
        //playAnimation
        //GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("遊戲... 開始!\n 正在播放：CommonEXEmusic");

        //Start play
        yield return new WaitForSeconds(4f);
        //如果要Random纖手玩家 這邊宣告
        turnToPlayer0 = true;
        startGameBool = true;
        endGameBool = false;
        AttackCall = true;
        yield return null;
    }

    public void hintWordBroadCast(string hintInformation)
    {
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint(hintInformation);
    }

    public void UpdatePlayer()
    {
        bool debugBlockPlayer0;//暫時先這樣 不過加入觀戰者會有bug
        if (Object.HasStateAuthority)
        {
            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(0))
            {
                p0 = Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>();
                p0.playerName = p0NameString;
                p0.myPlayerSort = 0;

                debugBlockPlayer0 = true;
            }

            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(1))
            {
                p1 = Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>();
                p1.playerName = p1NameString;
                p1.myPlayerSort = 1;
                Debug.Log("我知道我是1 等等聽起來有點怪怪的");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void FixedUpdateNetwork()
    {
        //capture player's information
        if (Object.HasStateAuthority)
        {
            if (p0 != null)
            {
                //同步
                if (p0.AtkFinishedCall)
                {
                    p0.AtkFinishedCall = false;
                    p0.AtkCallTired = false;
                    SwapAtkDir = p0.AtkDir;
                    SwapAtkStr = p0.AtkStr;
                    //由攻擊切換到防守
                    AttackCall = false;
                    DefendCall = true;

                    p0.AtkCallTired = false;
                    //StartCoroutine(breaker(p0,0));
                }
                if (p0.DefFinishedCall)
                {
                    //防守到結束
                    p0.DefFinishedCall = false;
                    SwapDefDir = p0.DefDir;
                    SwapABloack = p0.Ablock;
                    SwapCur = p0.Cur;
                    DefendCall = false;
                    //回合結算

                    //StartCoroutine(breaker(p0, 1));
                    p0.DefCallTired = false;

                    StartCoroutine(nextRound());
                }
            }

            if (p1 != null)
            {
                //同步
                if (p1.AtkFinishedCall)
                {
                    p1.AtkFinishedCall = false;
                    SwapAtkDir = p1.AtkDir;
                    SwapAtkStr = p1.AtkStr;
                    //由攻擊切換到防守
                    AttackCall = false;
                    DefendCall = true;
                    //StartCoroutine(breaker(p1, 0));

                    p1.AtkCallTired = false;
                }
                if (p1.DefFinishedCall)
                {
                    //防守到結束
                    p1.DefFinishedCall = false;
                    SwapDefDir = p1.DefDir;
                    SwapABloack = p1.Ablock;
                    SwapCur = p1.Cur;
                    DefendCall = false;
                    //回合結算

                    p1.DefCallTired = false;
                    //StartCoroutine(breaker(p1, 1));

                    StartCoroutine(nextRound());
                }
            }
        }


        if (startGameBool)
        {
            Debug.Log("遊戲進行中");
            if (endGameBool)//遊戲結束
            {
                Debug.Log("遊戲結束");
                if (!gameEndBlock)
                {
                    gameEndBlock = true;
                    //執行內容
                    bgmPlayer.Stop();
                    effectPlayer.Play();
                    blackFilter.SetActive(true);
                }
            }

            if (p0 != null)
            {
                killLIne0.transform.position = new Vector3(-p0.Hps*5.1f, 0f, 0f)+ UIpos.transform.position;
            }
            if (p1 != null)
            {
                killLine1.transform.position = new Vector3(p1.Hps* 5.1f, 0f, 0f) + UIpos.transform.position;
            }

            FramePos.transform.position = new Vector3(Frame * 5.1f, 0f, 0f) + UIpos.transform.position;
        }

        swapFrameShow.text = "現在架式：" + Frame;
    }
    
    IEnumerator nextRound()
    {
        //計算、同步
        float MaxmentRange = SwapAtkDir + SwapCur/2;
        float MinimentRange = SwapAtkDir - SwapCur/2;
        
        float ABlockMaxmentRange = SwapAtkDir + (SwapCur/2 * SwapABloack);
        float ABlockMinimentRange = SwapAtkDir - (SwapCur/2*SwapABloack);



        //精準格檔判斷
        if (ABlockMaxmentRange >= SwapDefDir && SwapDefDir >= ABlockMinimentRange)
        {
            SwapDamageType = 2;
        }
        else if (ABlockMaxmentRange > 360f)
        {
            if (SwapDefDir <= (ABlockMaxmentRange - 360f))
            {
                SwapDamageType = 2;
            }
        }
        else if (ABlockMinimentRange < 0f)
        {
            if ((ABlockMinimentRange + 360f) <= SwapDefDir)
            {
                SwapDamageType = 2;
            }
        }

        //普通格檔判斷
        else if (MaxmentRange >= SwapDefDir && SwapDefDir >= MinimentRange)
        {
            SwapDamageType = 1;
        }
        else if (MaxmentRange > 360f)
        {
            if (SwapDefDir <= (MaxmentRange - 360f))
            {
                SwapDamageType = 1;
            }
        }
        else if (MinimentRange < 0f)
        {
            if ((MinimentRange + 360f) <= SwapDefDir)
            {
                SwapDamageType = 1;
            }
        }
        //完整受傷
        else
        {
            SwapDamageType = 0;
        }


        //受傷判斷
        if (SwapDamageType == 2)
        {
            Debug.Log("精確格檔!");
            SwapFrameChange = SwapAtkStr * 0f;
            SwapEnergyRecover = 100f*1f;
        }
        else if (SwapDamageType == 1)
        {
            Debug.Log("部分格檔");
            //精準度計算
            if (MaxmentRange > 360f && MinimentRange > SwapDefDir)
            {
                SwapDefDir += 360f;
            }
            else if (MinimentRange < 0f && SwapDefDir > MaxmentRange) 
            {
                SwapDefDir -= 360f;
            }
            float AcureatPercent = Mathf.Abs(SwapAtkDir - SwapDefDir)*(1/SwapCur);
            Debug.Log("格檔精準率："+AcureatPercent);

            SwapFrameChange = SwapAtkStr * (1f - AcureatPercent);
            SwapEnergyRecover = 100f * AcureatPercent;
        }
        else if (SwapDamageType == 0)
        {
            Debug.Log("完全命中!");
            SwapFrameChange = SwapAtkStr * 1f;
            SwapEnergyRecover = 100f*0f;
        }

        //開始受傷與回合判斷
        if (turnToPlayer0)//玩家0攻擊
        {
            if (Mathf.Abs(Frame) > p1.Hps)
            {
                //斬殺P1
                Debug.Log("P1斬殺!");
                endGameBool = true;
            }
            Debug.Log("P1受到了"+SwapFrameChange+"點架式傷害!");
            Frame += SwapFrameChange;
            p1.Energy += SwapEnergyRecover;
        }
        else//玩家1攻擊
        {
            if (Mathf.Abs(Frame) > p0.Hps)
            {
                //斬殺P0
                Debug.Log("P0斬殺!");
                endGameBool = true;
            }
            Debug.Log("P0受到了" + SwapFrameChange + "點架式傷害!");
            Frame -= SwapFrameChange;
            p0.Energy += SwapEnergyRecover;
        }

        yield return new WaitForSeconds(nextRoundCoolDown);

        //下回合公告
        turnToPlayer0 = !turnToPlayer0;
        AttackCall = true;
        yield return null;
    }

    IEnumerator breaker(internetPlayer player,int kind)
    {
        yield return new WaitForSeconds(0.1f);
        if (kind == 0)
        {
            player.AtkCallTired = false;
        }
        else
        {
            player.DefCallTired = false;
        }
    }
}
