using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class gameIinker : MonoBehaviour, INetworkRunnerCallbacks
{
    bool isMeLogin = true;//在進入場景時為false 離開時為true 用於判斷是否是自己進入場景


    [SerializeField]
    private NetworkRunner networkRunner = null;

    [SerializeField]
    private NetworkPrefabRef playerPrefab;//玩家prefab

    Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();//玩家清單
    Dictionary<PlayerRef, string> playerNameList = new Dictionary<PlayerRef, string>();//玩家名字清單

    [Networked]
    public int playerNumber { get;set; }

    [SerializeField]
    Text player1Name;
    [SerializeField]
    Text player2Name;

    [SerializeField]
    public gameCore gameCores;//遊戲核心

    public internetPlayer myPlayer;

    public void InternetStartGame()
    {
        StartGame(GameMode.AutoHostOrClient);
    }

    async void StartGame(GameMode mode)
    {
        string sessionNameSwap = GameObject.Find("UI").transform.GetChild(1).GetComponent<InputField>().text;
        if (GameObject.Find("UI").transform.GetChild(1).GetComponent<InputField>().text == null)
        {
            sessionNameSwap = "DefultRoom";//防止玩家沒有輸入房間名
        }

        networkRunner.ProvideInput = true;//提供權限

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionNameSwap,
            //Scene = SceneManager.GetActiveScene().buildIndex,
            Scene = 2,//combat場景
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        Debug.Log("StartGame Async");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayeJoined");
        //gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Vector2.up, Quaternion.identity, player);

        playerList.Add(player, networkPlayerObject);
        //playerNameList.Add(player, playerList[player].GetComponent<playerInformation>().myName);//這邊等等看怎麼改

        //player1Name = GameObject.Find("P1Name").GetComponent<Text>();
        //player2Name = GameObject.Find("P2Name").GetComponent<Text>();

        //playerNumber++;

        //while (GameObject.Find("gameCore") == null)
        //{
        //    Debug.Log("Waiting to be capture");
        //}
        Debug.Log("Ready To capture...?");
        //waitingDuaSha();//偉大的capture!
        StartCoroutine(captureGameCore());
        //     gameCores.numberIntheScene++;

        //     Debug.Log("玩家人數：" + gameCores.numberIntheScene);
        // GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("玩家加入！");

        // if (!IsInvoking("cycleHint"))
        // {
        //     Invoke("cycleHint",2f);
        // }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("有玩家離開了遊戲");
        if (playerList.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            playerList.Remove(player);
            gameCores.numberIntheScene--;
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        var data = new playerInputData();
        var data2 = new playerInputData();

        data.playerSort = 10;
        data2.playerSort = 20;

        input.Set(data2);
        input.Set(data);

    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) {
        Debug.Log("OnConnectedToServer(NetworkRunner runner)");
    }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
        Debug.Log("OnConnect Request");
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) {
        Debug.Log("OnSceneLoadDone");
    }
    public void OnSceneLoadStart(NetworkRunner runner) 
    {
        Debug.Log("OnSceneLoadStart");
    }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    /*
    //將玩家歸類為P1或P2
    void sortingP1orP2()
    {
        if (playerNumber == 1)
        {
            //顯示在左邊
        }
        else if (playerNumber == 2)
        {
            //顯示在右邊
        }
    }*/

    async Task waitingToCaptureGameCore()
    {
        while (gameCores == null)
        {
            Debug.Log("Waiting to be capture");
            if (!!GameObject.Find("gameCore"))
            {
                gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
            }
        }
        Debug.Log("capture Susses!");
    }

    async void waitingDuaSha()
    {
        await waitingToCaptureGameCore();
    }

    void cycleHint()
    {
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("現在場景內玩家人數為"+gameCores.numberIntheScene);
        Invoke("cycleHint", 1f);
    }

    IEnumerator captureGameCore()
    {
        while (gameCores == null)
        {
            Debug.Log("Waiting to be capture");
            if (!!GameObject.Find("gameCore"))
            {
                gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
            }
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("capture Susses!");
        yield return null;
        gameCores.numberIntheScene++;
        Debug.Log("玩家人數：" + gameCores.numberIntheScene);
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("玩家加入！");

        if (isMeLogin)//是自己log in
        {
            isMeLogin = false;
            /*
                if (playerNumber == 1)
                {
                    player1Name.text = localDataBase.PlayerData.Name;
                }
                else if (playerNumber == 2)
                {
                    player2Name.text = localDataBase.PlayerData.Name;
                }
            */
            //playerNameList.Add(player, playerList[player].GetComponent<playerInformation>().myName);
            //HintWord_RPC(playerNameList[player]);

            //myPlayer = networkPlayerObject.GetComponent<internetPlayer>();//取得自己角色的playerObject
            //好像不能這樣用喔?
            if (gameCores.numberIntheScene == 1)
            {
                gameCores.p1NameString = localDataBase.PlayerData.Name;
                //gameCores.player1Name.text = localDataBase.PlayerData.Name;

                //gameCores.hintWordBroadCast("第" + gameCores.numberIntheScene + "位玩家 " + gameCores.p1NameString + " 加入了遊戲");
                //gameCores.namePlayer();
            }
            else if (gameCores.numberIntheScene == 2)
            {
                gameCores.p2NameString = localDataBase.PlayerData.Name;
                //gameCores.player2Name.text = localDataBase.PlayerData.Name;

                //gameCores.hintWordBroadCast("第" + gameCores.numberIntheScene + "位玩家 " + gameCores.p2NameString + " 加入了遊戲");
                //gameCores.namePlayer();
            }
        }

        if (gameCores.numberIntheScene == 1)
        {
            gameCores.hintWordBroadCast("第" + gameCores.numberIntheScene + "位玩家 " + gameCores.p1NameString + " 加入了遊戲");
            Debug.Log("玩家人數為1!");
        }
        if (gameCores.numberIntheScene == 2)
        {
            gameCores.hintWordBroadCast("第" + gameCores.numberIntheScene + "位玩家 " + gameCores.p2NameString + " 加入了遊戲");
            Debug.Log("玩家人數為2!");
        }
        gameCores.Rpc_namePlayer();
        if (gameCores.numberIntheScene >= 2)
        {
            Debug.Log("遊戲開始");//旁觀者模式加入後 這邊記得多加一個條件
            gameCores.reStartGame();
        }
    }
}
