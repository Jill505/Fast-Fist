using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirInputSolo : MonoBehaviour
{
    public GameObject arrow;
    public soloMob theMob;
    public soloCenter Center;

    //public float dir;//CNM
    public float allowRange;
    public float returnValue;

    public float ReactTime;

    float reactCountdown;

    public bool allowInputAtk;
    public bool allowInputDef;

    public float pressingTime;
    public Transform rectTransform;
    public Transform pt0;
    public Transform pt1;
    public Transform  pt2;
    public Transform pt3;

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
                Center.atkHintRing.SetTrigger("hide");
                //怪物受傷害
                theMob.mobDefence(Center.player.Str);

                allowInputAtk = !allowInputAtk;

                Center. NextRoundJudgement();

            }
            if (allowInputDef)
            {
                Center.playerReactMobNormalAttack(returnValue, theMob.mobdir);
                allowInputDef = !allowInputDef;

                Center.NextRoundJudgement();

                Center.theFist.speed = 1f;
                StopCoroutine(Center.touch());
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 滑鼠左鍵點擊事件
            Vector2 mousePosition = Input.mousePosition;
            Vector2 localPoint;

            if (mousePosition.x > pt0.position.x)
            {
                if (mousePosition.y > pt0.position.y)
                {
                    //1
                    Debug.Log("area1");
                }
                else if (mousePosition.y > pt2.position.y)
                {
                    //1
                    Debug.Log("area2");
                }
                else
                {
                    //1
                    Debug.Log("area3");
                }
            }
            else if (mousePosition.x > pt1.position.x)
            {
                if (mousePosition.y > pt0.position.y)
                {
                    //1
                    Debug.Log("area4");
                }
                else if (mousePosition.y > pt2.position.y)
                {
                    //1
                    Debug.Log("area5");
                }
                else
                {
                    //1
                    Debug.Log("area6");
                }
            }
            else 
            {
                if (mousePosition.y > pt0.position.y)
                {
                    //1
                    Debug.Log("area7");
                }
                else if (mousePosition.y > pt2.position.y)
                {
                    //1
                    Debug.Log("area8");
                }
                else
                {
                    //1
                    Debug.Log("area9");
                }
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
