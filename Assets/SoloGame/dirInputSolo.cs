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
    public RectTransform rectTransform;
    public RectTransform pt0;
    public RectTransform pt1;
    public RectTransform pt2;
    public RectTransform pt3;

    void Start()
    {
        Debug.Log("我最棒的DirInput開始執行執行了");
    }

    Vector2 lastPos;//鼠标上次位置
    Vector2 currPos;//鼠标当前位置
    Vector2 offset;//两次位置的偏移值

    void Update()
    {
        Debug.Log(pt0 + "RECT-" + pt0.rect.position + "   transfrom：" + pt0.transform.position);

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
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 滑鼠左鍵點擊事件
            Vector2 mousePosition = Input.mousePosition;
            Vector2 localPoint;

            // 將滑鼠位置轉換為 RectTransform 的本地座標
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out localPoint))
            {
                // 在這裡進行點擊位置的相關處理
                Debug.Log("Mouse clicked at local position: " + localPoint);
                Debug.Log(pt0 + "RECT-"+ pt0.rect.position + "   transfrom：" + pt0.transform.position);

                if (localPoint.x > pt0.rect.position.x)
                {
                    if (localPoint.y > pt0.rect.position.y)
                    {
                        //1
                        Debug.Log("area1");
                    }
                    else if (localPoint.y > pt2.rect.position.y)
                    {
                        //2
                        Debug.Log("area2");
                    }
                    else
                    {
                        //3
                        Debug.Log("area3");
                    }
                }
                else if (localPoint.x > pt1.rect.x)
                {
                    if (localPoint.y > pt0.rect.y)
                    {
                        //4
                        Debug.Log("area4");
                    }
                    else if (localPoint.y > pt2.rect.y)
                    {
                        //5
                        Debug.Log("area5");
                    }
                    else
                    {
                        //6
                        Debug.Log("area6");
                    }
                }
                else
                {
                    if (localPoint.y > pt0.rect.y)
                    {
                        //7
                        Debug.Log("area7");
                    }
                    else if (localPoint.y > pt2.rect.y)
                    {
                        //8
                        Debug.Log("area8");
                    }
                    else
                    {
                        //9
                        Debug.Log("area9");
                    }
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
