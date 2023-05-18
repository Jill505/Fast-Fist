using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class commanderToChnageImage : MonoBehaviour
{

    public bool changePicNormal,changePicAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (changePicNormal)
        {
            changePicNormal = false;
            orderToChangePic();
        }

        if (changePicAttack)
        {
            changePicAttack = false;
            orderToChangePicAtk();
        }
    }

    public void orderToChangePic()
    {
        transform.GetChild(0).gameObject.GetComponent<soloMob>().changeToNormalPic();
    }
    public void orderToChangePicAtk()
    {
        transform.GetChild(0).gameObject.GetComponent<soloMob>().changeToAttackPic();
    }
}
