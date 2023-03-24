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

    public Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();//���a�M��
    public Dictionary<PlayerRef, string> playerNameList = new Dictionary<PlayerRef, string>();//���a�W�r�M��
    public Dictionary<int,PlayerRef> playerIDList = new Dictionary<int, PlayerRef>();//���aID�M��

    [SerializeField]
    Text player1Name;
    [SerializeField]
    Text player2Name;

    [SerializeField]
    public gameCore gameCores;//�C���֤�

    //���a�I�s��
    public bool TimeToUpload;
    public bool brocastedName = false;
    public bool AtkCall = false;//��������Ĳ�o��
    public bool DefCall = false;//���u����Ĳ�o��

    //���a������ܼ�
    public internetPlayer myPlayer;//���a���a
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
            Debug.Log("����L���a�[�J��");
            Debug.Log(gameCores.p1NameString+"�[�J�F�C��");

            if (gameCores.HasStateAuthority)
            {
                //���o�v��
            }
        }
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
        if (MyDirInput == null)
        {
            if (GameObject.Find("DirInputSystem"))
            {
                MyDirInput = GameObject.Find("DirInputSystem").GetComponent<DirInput>();
            }
        }
        else
        {
            //����DirInput�ƾڤW��
        }

        if (myPlayer == null)
        {
            //waiting
        }
        else
        {
            //����interNetPlayer�ƾڤW��

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
            Debug.Log("�ϩR����! input���a���!");
        }

        if (TimeToUpload)
        {
            var dataMinas1 = new TestStruck();
            dataMinas1.uploadInt = 1;
            input.Set(dataMinas1);
            TimeToUpload = false;
            Debug.Log("��ƤW��");
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
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("�{�b���������a�H�Ƭ�"+gameCores.numberIntheScene);
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
        //Debug.Log("���a�H�ơG" + gameCores.numberIntheScene);
        //GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("���a�[�J�I");

        gameCores.numberIntheScene++;
        Debug.Log("���a�H�ơG" + gameCores.numberIntheScene);

        if (isMeLogin)//�O�ۤvlog in
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

            playerComponentInternetPlayer.CharacterGivingValue(localDataBase.PlayerData.selectionCharacter);//��ܨ�����*/
        }

        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("���a�[�J�I");

        gameCores.UpdatePlayer();

        yield return new WaitForSeconds(0.5f);

        if (gameCores.numberIntheScene == 1)
        {
            gameCores.hintWordBroadCast("��" + gameCores.numberIntheScene + "�쪱�a " + gameCores.p0NameString + " �[�J�F�C��");
            Debug.Log("���a�H�Ƭ�1!");
        }
        if (gameCores.numberIntheScene == 2)
        {
            gameCores.hintWordBroadCast("��" + gameCores.numberIntheScene + "�쪱�a " + gameCores.p1NameString + " �[�J�F�C��");
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
