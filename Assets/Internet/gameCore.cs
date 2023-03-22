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

    //共用數據-架式
    [Networked] public float Frame { get; set; }

    //回合歸屬(誰為攻方)
    [Networked] public bool turnToPlayer0{ get; set; }

    //我自己取的名字! "網路板機"
    //狀態機一律以player1 為true時觸發 player2相反
    [Networked] public bool AttackCall { get; set; }
    [Networked] public bool DefendCall { get; set; }

    //開始呼叫
    [Networked] public bool startGameBool { get; set; }

    public AudioSource bgmPlayer;

    public Text player0Name;
    public Text player1Name;

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

    public void reStartGame()
    {
        Frame = 0f;
        StartCoroutine(startGameIEnumerator());
    }

    IEnumerator startGameIEnumerator()
    {
        yield return new WaitForSeconds(3f);
        //playMusic
        bgmPlayer.clip = Resources.Load<AudioClip>("DelFile/CommonEXEmusic");//Capture BGM
        bgmPlayer.Play();
        //playAnimation
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("遊戲... 開始!\n 正在播放：CommonEXEmusic");

        //Start play
        //如果要Random纖手玩家 這邊宣告
        turnToPlayer0 = true;
        startGameBool = true;
        AttackCall = true;
        yield return null;
    }

    public void hintWordBroadCast(string hintInformation)
    {
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint(hintInformation);
    }

    public void UpdatePlayer()
    {
        if (Object.HasStateAuthority)
        {
            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(0))
            {
                p0 = Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>();
                p0.playerName = p0NameString;
                p0.myPlayerSort = 0;
            }
            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(1))
            {
                p1 = Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>();
                p1.playerName = p1NameString;
                p1.myPlayerSort = 1;
            }
        }
    }

    void Start()
    {

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

        }
    }
}
