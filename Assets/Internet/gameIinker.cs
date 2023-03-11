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
    [SerializeField]
    private NetworkRunner networkRunner = null;

    [SerializeField]
    private NetworkPrefabRef playerPrefab;

    Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();//玩家清單
    Dictionary<PlayerRef, string> playerNameList = new Dictionary<PlayerRef, string>();//玩家名字清單

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
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Vector2.up, Quaternion.identity, player);

        playerList.Add(player, networkPlayerObject);
        playerNameList.Add(player, playerList[player].GetComponent<playerInformation>().myName);//這邊等等看怎麼改
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        /*if (playerList.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            playerList.Remove(player);
        }*/
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
}
