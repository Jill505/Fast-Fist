using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class interNetTester : NetworkBehaviour
{
    public int myPlayerSort;
    public override void FixedUpdateNetwork()
    {

        syncTestText.text = "�����[���G\n" + testNumber;

        if (Object.HasStateAuthority) 
        {
            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(0))//p1
            {
                if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>().TestVariable != 0)
                {
                    testNumber += Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>().TestVariable;
                    Debug.Log("��J�F" + Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>().TestVariable);
                    Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>().TestVariable = 0;
                }
            }
            /*
            if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable != 0)//��ۤv
            {
                testNumber += Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable;
                Debug.Log("��J�F" + Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable);
                Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable = 0;
            }
            */
            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(1))//p2
            {
                if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>().TestVariable != 0)
                {
                    testNumber += Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>().TestVariable;
                    Debug.Log("��J�F" + Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>().TestVariable);
                    Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>().TestVariable = 0;
                }
            }
        }

        Debug.Log("�Ӧ۽r�r��:"+Runner.gameObject.GetComponent<gameIinker>().playerList);

        foreach (KeyValuePair<PlayerRef, NetworkObject> kvp in Runner.gameObject.GetComponent<gameIinker>().playerList)
        {
            Debug.Log(kvp.Key + "  " + kvp.Value);
        }

        foreach (KeyValuePair<int, PlayerRef> kvp in Runner.gameObject.GetComponent<gameIinker>().playerIDList)
        {
            Debug.Log(kvp.Key + "  �o�O�Ӧ۽r�r�ժ��̫�i��  " + kvp.Value);
        }

        /*
        if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].HasInputAuthority)
        {
            Debug.Log("�ڦ��ۤv����J�v��");
        }*/
        /*if (Runner.gameObject.GetComponent<gameIinker>().playerList.ContainsKey(Runner.LocalPlayer))
        {
            if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].HasStateAuthority)
            {
                Debug.Log("�ڦ��ۤv�����A�v��");
            }
        }*/

        Debug.Log(Runner.name);
        Debug.Log(GameObject.Find("gameCore").GetComponent<gameCore>().numberIntheScene);

    }

    [Networked]
    public int testNumber { get; set; }
    public Text syncTestText;
    public void AddNumberTest()
    {
        Debug.Log(GameObject.Find("NetworkRunner").GetComponent<gameIinker>().playerList);
        //Debug.Log(GameObject.Find("NetworkRunner").GetComponent<gameIinker>().playerList[Runner.LocalPlayer]);

        if (GameObject.Find("NetworkRunner").GetComponent<gameIinker>().playerList.TryGetValue(Runner.LocalPlayer, out NetworkObject networkObject))
        {
            //networkObject.gameObject.GetComponent<internetPlayer>().TestVariable++;
            Debug.Log("�j�a�i�H�ۭ}�F");
        }
        else
        {
            Debug.Log("���F�S��� �f�i");
        }

        Runner.gameObject.GetComponent<gameIinker>().TimeToUpload = true;

        //GameObject.Find("NetworkRunner").GetComponent<gameIinker>().GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable++;
        //if (GameObject.Find("NetworkRunner").GetComponent<gameIinker>().GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable == 0)
        //{
        //    Debug.Log("�A�٬O��異�Ѱ�");
        //}
        //else if (GameObject.Find("NetworkRunner").GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable == 1)
        //{
        //    Debug.Log("�ڭ�... �ڭ̦��\�F!");
        //}




        /*
        if (!gameObject.GetComponent<NetworkObject>().HasInputAuthority)
        {
            Debug.Log("�p�G�M���O�ظo ���ګK�S��inputAuthority");
        }
        else
        {
            Debug.Log("Input�L�o����");
        }

        if (!gameObject.GetComponent<NetworkObject>().HasStateAuthority)
        {
            Debug.Log("�p�G�M���O�ظo ���ګK�S��StateAuthority");
        }
        else
        {
            Debug.Log("State�L�o����");
        }

        testNumber++;
        Debug.Log("numberAdded");

        gameObject.GetComponent<NetworkObject>().AssignInputAuthority(Runner.LocalPlayer);*/
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     //Input.   
    }
}
