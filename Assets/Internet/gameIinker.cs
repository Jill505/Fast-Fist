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

        Debug.Log("StartGame Async");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayeJoined");
        //gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Vector2.up, Quaternion.identity, player);

        playerList.Add(player, networkPlayerObject);
        //playerNameList.Add(player, playerList[player].GetComponent<playerInformation>().myName);//�o�䵥���ݫ���

        //player1Name = GameObject.Find("P1Name").GetComponent<Text>();
        //player2Name = GameObject.Find("P2Name").GetComponent<Text>();

        //playerNumber++;

        //while (GameObject.Find("gameCore") == null)
        //{
        //    Debug.Log("Waiting to be capture");
        //}
        Debug.Log("Ready To capture...?");
        //waitingDuaSha();//���j��capture!
        StartCoroutine(captureGameCore());
        //     gameCores.numberIntheScene++;

        //     Debug.Log("���a�H�ơG" + gameCores.numberIntheScene);
        // GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("���a�[�J�I");

        // if (!IsInvoking("cycleHint"))
        // {
        //     Invoke("cycleHint",2f);
        // }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("�����a���}�F�C��");
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
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("�{�b���������a�H�Ƭ�"+gameCores.numberIntheScene);
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
        Debug.Log("���a�H�ơG" + gameCores.numberIntheScene);
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("���a�[�J�I");

        if (isMeLogin)//�O�ۤvlog in
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

            //myPlayer = networkPlayerObject.GetComponent<internetPlayer>();//���o�ۤv���⪺playerObject
            //�n������o�˥γ�?
            if (gameCores.numberIntheScene == 1)
            {
                gameCores.p1NameString = localDataBase.PlayerData.Name;
                //gameCores.player1Name.text = localDataBase.PlayerData.Name;

                //gameCores.hintWordBroadCast("��" + gameCores.numberIntheScene + "�쪱�a " + gameCores.p1NameString + " �[�J�F�C��");
                //gameCores.namePlayer();
            }
            else if (gameCores.numberIntheScene == 2)
            {
                gameCores.p2NameString = localDataBase.PlayerData.Name;
                //gameCores.player2Name.text = localDataBase.PlayerData.Name;

                //gameCores.hintWordBroadCast("��" + gameCores.numberIntheScene + "�쪱�a " + gameCores.p2NameString + " �[�J�F�C��");
                //gameCores.namePlayer();
            }
        }

        if (gameCores.numberIntheScene == 1)
        {
            gameCores.hintWordBroadCast("��" + gameCores.numberIntheScene + "�쪱�a " + gameCores.p1NameString + " �[�J�F�C��");
            Debug.Log("���a�H�Ƭ�1!");
        }
        if (gameCores.numberIntheScene == 2)
        {
            gameCores.hintWordBroadCast("��" + gameCores.numberIntheScene + "�쪱�a " + gameCores.p2NameString + " �[�J�F�C��");
            Debug.Log("���a�H�Ƭ�2!");
        }
        gameCores.Rpc_namePlayer();
        if (gameCores.numberIntheScene >= 2)
        {
            Debug.Log("�C���}�l");//���[�̼Ҧ��[�J�� �o��O�o�h�[�@�ӱ���
            gameCores.reStartGame();
        }
    }
}
