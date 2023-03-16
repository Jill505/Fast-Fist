using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

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
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayeJoined");
        //gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Vector2.up, Quaternion.identity, player);

        playerList.Add(player, networkPlayerObject);
        //playerNameList.Add(player, playerList[player].GetComponent<playerInformation>().myName);//這邊等等看怎麼改

        player1Name = GameObject.Find("P1Name").GetComponent<Text>();
        player2Name = GameObject.Find("P2Name").GetComponent<Text>();

        playerNumber++;
        gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
        gameCores.numberIntheScene++;

        Debug.Log("玩家人數：" + playerNumber);

        if (isMeLogin)//是自己log in
        {
            isMeLogin = false;

            /*if (playerNumber == 1)
            {
                player1Name.text = localDataBase.PlayerData.Name;
            }
            else if (playerNumber == 2)
            {
                player2Name.text = localDataBase.PlayerData.Name;
            }*/

            //playerNameList.Add(player, playerList[player].GetComponent<playerInformation>().myName);
            //HintWord_RPC(playerNameList[player]);

            myPlayer = networkPlayerObject.GetComponent<internetPlayer>();//取得自己角色的playerObject

            if (gameCores.numberIntheScene == 1)
            {
                gameCores.p1NameString = localDataBase.PlayerData.Name;
                //gameCores.player1Name.text = localDataBase.PlayerData.Name;

                gameCores.hintWordBroadCast("第" + gameCores.numberIntheScene + "位玩家 " + gameCores.p1NameString + " 加入了遊戲");
                gameCores.namePlayer();
            }
            else if (gameCores.numberIntheScene == 2)
            {
                gameCores.p2NameString = localDataBase.PlayerData.Name;
                //gameCores.player2Name.text = localDataBase.PlayerData.Name;

                gameCores.hintWordBroadCast("第" + gameCores.numberIntheScene + "位玩家 " + gameCores.p2NameString + " 加入了遊戲");
                gameCores.namePlayer();
            }

            if (gameCores.numberIntheScene == 2)
            {
                Debug.Log("遊戲開始");
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
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


    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void HintWord_RPC(string playerName)
    {
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("第" + playerNumber + "位玩家 "+ playerName + " 加入了遊戲");
    }
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
}
