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
        if (GetInput(out playerInputData data))
        {
            Debug.Log("�Ӧ�interNetTester�G"+data.playerSort);
            myPlayerSort = data.playerSort;
        }
    }

    [Networked]
    public int testNumber { get; set; }
    public Text syncTestText;
    public void AddNumberTest()
    {
        testNumber++;
        syncTestText.text = "�P�B�Ʀr�G\n"+testNumber;
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
