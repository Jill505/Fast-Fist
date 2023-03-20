using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class gameCore : NetworkBehaviour
{
    [Networked] public int numberIntheScene { get; set; }

    [Networked] public string p1NameString { get; set; }
    [Networked] public string p2NameString { get; set; }

    //共用數據-架式
    [Networked] public float Frame { get; set; }

    //回合歸屬(誰為攻方)
    [Networked] public bool turnToPlayer1 { get; set; }

    //我自己取的名字! "網路板機"
    //狀態機一律以player1 為true時觸發 player2相反
    [Networked] public bool TimeToAttakc { get; set; }
    [Networked] public bool TimeToDefend { get; set; }

    public AudioSource bgmPlayer;

    public Text player1Name;
    public Text player2Name;

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_namePlayer()
    {
        player1Name.text = p1NameString;
        player2Name.text = p2NameString;
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
        yield return null;
    }

    public void hintWordBroadCast(string hintInformation)
    {
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint(hintInformation);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
