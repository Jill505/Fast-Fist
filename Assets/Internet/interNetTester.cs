using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class interNetTester : NetworkBehaviour
{
    public int myPlayerSort;
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out playerInputData data))
        {
            Debug.Log(data.playerSort);
            myPlayerSort = data.playerSort;
        }
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
