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

    public Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();//玩家清單
    public Dictionary<PlayerRef, string> playerNameList = new Dictionary<PlayerRef, string>();//玩家名字清單
    public Dictionary<int,PlayerRef> playerIDList = new Dictionary<int, PlayerRef>();//玩家ID清單

    [SerializeField]
    Text player1Name;
    [SerializeField]
    Text player2Name;

    [SerializeField]
    public gameCore gameCores;//遊戲核心

    //本地呼叫器
    public bool TimeToUpload;
    public bool brocastedName = false;
    public bool AtkCall = false;//攻擊完成觸發器
    public bool DefCall = false;//防守完成觸發器

    //本地抓取的變數
    public internetPlayer myPlayer;//本地玩家
    public DirInput MyDirInput;
    PlayerRef myPlayerRef;

    public void InternetStartGame()
    {
        StartGame(GameMode.AutoHostOrClient);
        //StartGame(GameMode.Shared);
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
        //InterNetSwitch
        GameObject myGameObject;

        Debug.Log("OnPlayeJoined");
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Vector2.up, Quaternion.identity, player);
        //myGameObject = networkPlayerObject.gameObject;

        playerList.Add(player, networkPlayerObject);
        //playerIDList.Add(gameCores.numberIntheScene,player);
        Debug.Log("Ready To capture...?");
        StartCoroutine(captureGameCore(player));

        myPlayerRef = player;

        if (!isMeLogin)
        {
            Debug.Log("有其他玩家加入辣");
            Debug.Log(gameCores.p1NameString+"加入了遊戲");

            if (gameCores.HasStateAuthority)
            {
                //分發權限
            }
        }
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
        if (MyDirInput == null)
        {
            if (GameObject.Find("DirInputSystem"))
            {
                MyDirInput = GameObject.Find("DirInputSystem").GetComponent<DirInput>();
            }
        }
        else
        {
            //執行DirInput數據上傳
        }

        if (myPlayer == null)
        {
            //waiting
        }
        else
        {
            //執行interNetPlayer數據上傳

            if (AtkCall)
            {
                var data2 = new AtkInputData();

                data2.AtkDir = myPlayer.AtkDir;
                data2.AtkStr = myPlayer.Str;
                data2.AtkFinished = true;
            }
        }


        if (!brocastedName)
        {
            var data0 = new playerInputData();
            data0.characterSelectionSort = localDataBase.PlayerData.selectionCharacter;
            data0.moodSelectionSort = localDataBase.PlayerData.selectionMoodsort;
            input.Set(data0);
            Debug.Log("使命完成! input玩家資料!");
        }

        if (TimeToUpload)
        {
            var dataMinas1 = new TestStruck();
            dataMinas1.uploadInt = 1;
            input.Set(dataMinas1);
            TimeToUpload = false;
            Debug.Log("資料上載");
        }
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

    IEnumerator captureGameCore(PlayerRef player)
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
        playerIDList.Add(gameCores.numberIntheScene, player);
        yield return null;
        //gameCores.numberIntheScene++;
        //Debug.Log("玩家人數：" + gameCores.numberIntheScene);
        //GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("玩家加入！");

        gameCores.numberIntheScene++;
        Debug.Log("玩家人數：" + gameCores.numberIntheScene);

        if (isMeLogin)//是自己log in
        {
            MyDirInput = GameObject.Find("DirInputSystem").GetComponent<DirInput>();

            isMeLogin = false;
            /*internetPlayer playerComponentInternetPlayer;
            playerComponentInternetPlayer = swapGameObject.GetComponent<internetPlayer>();

            if (gameCores.numberIntheScene == 1)
            {
                gameCores.p1NameString = localDataBase.PlayerData.Name;
                playerComponentInternetPlayer.myPlayerSort = 1;
            }
            else if (gameCores.numberIntheScene == 2)
            {
                gameCores.p2NameString = localDataBase.PlayerData.Name;
                playerComponentInternetPlayer.myPlayerSort = 2;
            }

            playerComponentInternetPlayer.CharacterGivingValue(localDataBase.PlayerData.selectionCharacter);//選擇角色賦值*/
        }

        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("玩家加入！");

        gameCores.UpdatePlayer();

        yield return new WaitForSeconds(0.5f);

        if (gameCores.numberIntheScene == 1)
        {
            gameCores.hintWordBroadCast("第" + gameCores.numberIntheScene + "位玩家 " + gameCores.p0NameString + " 加入了遊戲");
            Debug.Log("玩家人數為1!");
        }
        if (gameCores.numberIntheScene == 2)
        {
            gameCores.hintWordBroadCast("第" + gameCores.numberIntheScene + "位玩家 " + gameCores.p1NameString + " 加入了遊戲");
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
