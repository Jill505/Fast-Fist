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
    bool isMeLogin = true;//�b�i�J�����ɬ�false ���}�ɬ�true �Ω�P�_�O�_�O�ۤv�i�J����


    [SerializeField]
    private NetworkRunner networkRunner = null;

    [SerializeField]
    private NetworkPrefabRef playerPrefab;//���aprefab

    Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();//���a�M��
    Dictionary<PlayerRef, string> playerNameList = new Dictionary<PlayerRef, string>();//���a�W�r�M��

    [Networked]
    public int playerNumber { get;set; }

    [SerializeField]
    Text player1Name;
    [SerializeField]
    Text player2Name;

    [SerializeField]
    public gameCore gameCores;//�C���֤�

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
            sessionNameSwap = "DefultRoom";//����a�S����J�ж��W
        }

        networkRunner.ProvideInput = true;//�����v��

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionNameSwap,
            //Scene = SceneManager.GetActiveScene().buildIndex,
            Scene = 2,//combat����
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayeJoined");
        //gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Vector2.up, Quaternion.identity, player);

        playerList.Add(player, networkPlayerObject);
        //playerNameList.Add(player, playerList[player].GetComponent<playerInformation>().myName);//�o�䵥���ݫ���

        player1Name = GameObject.Find("P1Name").GetComponent<Text>();
        player2Name = GameObject.Find("P2Name").GetComponent<Text>();

        playerNumber++;
        gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
        gameCores.numberIntheScene++;

        Debug.Log("���a�H�ơG" + playerNumber);

        if (isMeLogin)//�O�ۤvlog in
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

            myPlayer = networkPlayerObject.GetComponent<internetPlayer>();//���o�ۤv���⪺playerObject

            if (gameCores.numberIntheScene == 1)
            {
                gameCores.p1NameString = localDataBase.PlayerData.Name;
                //gameCores.player1Name.text = localDataBase.PlayerData.Name;

                gameCores.hintWordBroadCast("��" + gameCores.numberIntheScene + "�쪱�a " + gameCores.p1NameString + " �[�J�F�C��");
                gameCores.namePlayer();
            }
            else if (gameCores.numberIntheScene == 2)
            {
                gameCores.p2NameString = localDataBase.PlayerData.Name;
                //gameCores.player2Name.text = localDataBase.PlayerData.Name;

                gameCores.hintWordBroadCast("��" + gameCores.numberIntheScene + "�쪱�a " + gameCores.p2NameString + " �[�J�F�C��");
                gameCores.namePlayer();
            }

            if (gameCores.numberIntheScene == 2)
            {
                Debug.Log("�C���}�l");
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
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("��" + playerNumber + "�쪱�a "+ playerName + " �[�J�F�C��");
    }
    /*
    //�N���a�k����P1��P2
    void sortingP1orP2()
    {
        if (playerNumber == 1)
        {
            //��ܦb����
        }
        else if (playerNumber == 2)
        {
            //��ܦb�k��
        }
    }*/
}
