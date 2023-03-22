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

        syncTestText.text = "模擬架式：\n" + testNumber;

        if (Object.HasStateAuthority) 
        {
            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(0))//p1
            {
                if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>().TestVariable != 0)
                {
                    testNumber += Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>().TestVariable;
                    Debug.Log("輸入了" + Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>().TestVariable);
                    Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>().TestVariable = 0;
                }
            }
            /*
            if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable != 0)//抓自己
            {
                testNumber += Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable;
                Debug.Log("輸入了" + Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable);
                Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable = 0;
            }
            */
            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(1))//p2
            {
                if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>().TestVariable != 0)
                {
                    testNumber += Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>().TestVariable;
                    Debug.Log("輸入了" + Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>().TestVariable);
                    Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>().TestVariable = 0;
                }
            }
        }

        Debug.Log("來自緝毒組:"+Runner.gameObject.GetComponent<gameIinker>().playerList);

        foreach (KeyValuePair<PlayerRef, NetworkObject> kvp in Runner.gameObject.GetComponent<gameIinker>().playerList)
        {
            Debug.Log(kvp.Key + "  " + kvp.Value);
        }

        foreach (KeyValuePair<int, PlayerRef> kvp in Runner.gameObject.GetComponent<gameIinker>().playerIDList)
        {
            Debug.Log(kvp.Key + "  這是來自緝毒組的最後波紋  " + kvp.Value);
        }

        /*
        if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].HasInputAuthority)
        {
            Debug.Log("我有自己的輸入權限");
        }*/
        /*if (Runner.gameObject.GetComponent<gameIinker>().playerList.ContainsKey(Runner.LocalPlayer))
        {
            if (Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.LocalPlayer].HasStateAuthority)
            {
                Debug.Log("我有自己的狀態權限");
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
            Debug.Log("大家可以蹦迪了");
        }
        else
        {
            Debug.Log("阿幹沒找到 口可");
        }

        Runner.gameObject.GetComponent<gameIinker>().TimeToUpload = true;

        //GameObject.Find("NetworkRunner").GetComponent<gameIinker>().GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable++;
        //if (GameObject.Find("NetworkRunner").GetComponent<gameIinker>().GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable == 0)
        //{
        //    Debug.Log("你還是更改失敗啦");
        //}
        //else if (GameObject.Find("NetworkRunner").GetComponent<gameIinker>().playerList[Runner.LocalPlayer].gameObject.GetComponent<internetPlayer>().TestVariable == 1)
        //{
        //    Debug.Log("我們... 我們成功了!");
        //}




        /*
        if (!gameObject.GetComponent<NetworkObject>().HasInputAuthority)
        {
            Debug.Log("如果清醒是種罪 那我便沒有inputAuthority");
        }
        else
        {
            Debug.Log("Input無罪釋放");
        }

        if (!gameObject.GetComponent<NetworkObject>().HasStateAuthority)
        {
            Debug.Log("如果清醒是種罪 那我便沒有StateAuthority");
        }
        else
        {
            Debug.Log("State無罪釋放");
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
