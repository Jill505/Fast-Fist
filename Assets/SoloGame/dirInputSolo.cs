using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirInputSolo : MonoBehaviour
{
    public GameObject arrow;
    public soloMob theMob;

    //public float dir;//CNM
    public float allowRange;
    public float returnValue;

    public float ReactTime;

    float reactCountdown;

    public bool allowInputAtk;
    public bool allowInputDef;

    public float pressingTime;
    
    
    void Start()
    {
        Debug.Log("我最棒的DirInput開始執行執行了");
    }

    Vector2 lastPos;//鼠标上次位置
    Vector2 currPos;//鼠标当前位置
    Vector2 offset;//两次位置的偏移值

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
            pressingTime += 1f * Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            currPos = Input.mousePosition;
            offset = currPos - lastPos;
            returnValue = angle(lastPos, currPos);

            Debug.Log("原始角度為：" + returnValue);
            //DoMatch(offset);

            //發出訊告 告知完成了
            if (allowInputAtk)
            {

            }
            if (allowInputDef)
            {

            }
        }
    }

    void DoMatch(Vector2 _offset)
    {
        //水平判斷
        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
        {
            if (offset.x > 0)
            {
                Debug.Log("右");
            }
            else
            {
                Debug.Log("左");
            }
        }
        else//垂直判斷
        {
            if (offset.y > 0)
            {
                Debug.Log("上");
            }
            else
            {
                Debug.Log("下");
            }
        }
        //makeSure();
    }

    float angle(Vector2 from, Vector2 to)
    {
        Vector3 swapVector = (to - from).normalized;
        float swap = Mathf.Atan2(swapVector.y, swapVector.x) * Mathf.Rad2Deg;
        if (swap < 0)
        {
            swap += 360f;
        }
        return swap;
    }

    void FixedUpdate()
    {
        reactCountdown -= Time.fixedDeltaTime;
    }
}
