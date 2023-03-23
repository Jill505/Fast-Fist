using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirInput : MonoBehaviour
{
    public GameObject arrow;

    public float dir;
    public float allowRange;
    public float returnValue;

    public float ReactTime;

    public gameIinker gameRunner;
    public gameCore gameCores;
    public internetPlayer myInternetPlayer;

    float reactCountdown;

    // Start is called before the first frame update
    void Start()
    {
        allowRange = 50f;
        //("makeDis", 0f);

        gameRunner = GameObject.Find("NetworkRunner").GetComponent<gameIinker>();
        StartCoroutine(captureCoruotine());
    }

    Vector2 lastPos;//鼠标上次位置
    Vector2 currPos;//鼠标当前位置
    Vector2 offset;//两次位置的偏移值

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            currPos = Input.mousePosition;
            offset = currPos - lastPos;
            returnValue = angle(lastPos, currPos);
            DoMatch(offset);
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
        makeSure();
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


    void makeDis()
    {
        dir = Random.Range(0f, 360f);

        //arrow.transform.rotation = Quaternion.Euler(0f, 0f, dir);
        //Invoke("makeDis", ReactTime);
        reactCountdown = ReactTime;
    }

    void FixedUpdate()
    {
        reactCountdown -= Time.fixedDeltaTime;
    }

    void makeSure()
    {
        float rangeMax = dir + allowRange;
        float rangeMin = dir - allowRange;

        Debug.Log(rangeMin + "rf" + rangeMax);

        Debug.Log("角度為："+returnValue);

        if (returnValue <= rangeMax && returnValue >= rangeMin)
        {
            Debug.Log("ya");
        }
        else if (rangeMax > 360 && rangeMax - 360f > returnValue)
        {
            Debug.Log("ya2");
        }
        else if (rangeMin < 0 && rangeMin + 360f < returnValue)
        {
            Debug.Log("ya3");
        }
        else
        {
            Debug.Log("NO");
        }
    }

    IEnumerator captureCoruotine()
    {
        while (gameCores == null)
        {
            if (GameObject.Find("gameCores"))
            {
                gameCores = GameObject.Find("gameCores").GetComponent<gameCore>();
            }
            yield return new WaitForSeconds(0.025f);
        }

        while (myInternetPlayer == null)
        {
            myInternetPlayer = gameRunner.myPlayer;
            yield return new WaitForSeconds(0.025f);
        }

        yield return null;
    }
}
