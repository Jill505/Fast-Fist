using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soloCenter : MonoBehaviour
{
    public bool turnToPlayer = true;
    public bool casting;

    public soloStageManager stageManager;
    public soloPlayer player;
    public soloMob mob;
    public dirInputSolo DirInputSolo;

    public bool gameStarted;

    public Animator startGameAnimatoer;
    public Animator atkHintRing;
    public GameObject Fist;
    public Animator defHintFist;

    public float Frame;

    public int stageSort;//關卡序列

    public Text mobNameText;

    public float mobAttackDir;

    public IEnumerator gameStart()
    {
        if (stageSort == 0)
        {
            //實驗場
            //mob.mobSort = 0;
            //mobNameText.text = mob.mobName;
        }

        gameStarted = true;
        //開始播放音樂
        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        //開場動畫
        StartCoroutine(gameStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            if (turnToPlayer)
            {
                if (!casting)
                {
                    //讓玩家行動(攻擊或放大)
                    casting = true;
                    DirInputSolo.allowInputAtk = true;
                    //顯示攻擊圈
                }
                //等待
            }
            else
            {
                if (!casting)
                {
                    //mob行動
                    casting = true;
                    mob.mobAttack();
                }
                //等待
            }
        }
    }
    private void LateUpdate()
    {
        //顯示架式
    }


    //怪物普通攻擊接口
    public void mobNormalAttack(float dir)
    {
        DirInputSolo.allowInputDef= true;
        Fist.transform.localEulerAngles = new Vector3(0,0,dir);
        defHintFist.SetTrigger("show");
        //Animation Active
    }

    public void playerReactMobNormalAttack(float playerReturnDir, float mobDir)
    {
        defHintFist.SetTrigger("hide");

        //計算、同步
        float MaxmentRange = mobDir + player.Cur / 2;
        float MinimentRange = mobDir - player.Cur / 2;

        float ABlockMaxmentRange = mobDir + (player.Cur / 2 * player.Ablock);
        float ABlockMinimentRange = mobDir - (player.Cur / 2 * player.Ablock);

        int SwapDamageType = 0;

        //精準格檔判斷
        if (ABlockMaxmentRange >= playerReturnDir && playerReturnDir >= ABlockMinimentRange)
        {
            SwapDamageType = 2;
        }
        else if (ABlockMaxmentRange > 360f)
        {
            if (playerReturnDir <= (ABlockMaxmentRange - 360f))
            {
                SwapDamageType = 2;
            }
        }
        else if (ABlockMinimentRange < 0f)
        {
            if ((ABlockMinimentRange + 360f) <= playerReturnDir)
            {
                SwapDamageType = 2;
            }
        }

        //普通格檔判斷
        else if (MaxmentRange >= playerReturnDir && playerReturnDir >= MinimentRange)
        {
            SwapDamageType = 1;
        }
        else if (MaxmentRange > 360f)
        {
            if (playerReturnDir <= (MaxmentRange - 360f))
            {
                SwapDamageType = 1;
            }
        }
        else if (MinimentRange < 0f)
        {
            if ((MinimentRange + 360f) <= playerReturnDir)
            {
                SwapDamageType = 1;
            }
        }
        //完整受傷
        else
        {
            SwapDamageType = 0;
        }

        //受傷判斷
        if (SwapDamageType == 2)
        {
            Debug.Log("精確格檔!");
            Frame -= mobDir * 0f;
            player.Energy += 100f * 1f;


        }
        else if (SwapDamageType == 1)
        {
            Debug.Log("部分格檔");
            //精準度計算
            if (MaxmentRange > 360f && MinimentRange > playerReturnDir)
            {
                playerReturnDir += 360f;
            }
            else if (MinimentRange < 0f && playerReturnDir > MaxmentRange)
            {
                playerReturnDir -= 360f;
            }
            float AcureatPercent = Mathf.Abs(mobDir - playerReturnDir) * (1 / player.Cur);
            Debug.Log("格檔精準率：" + AcureatPercent);

            Frame -= mobDir * (1f - AcureatPercent);
            player.Energy += 100f * AcureatPercent;


        }
        else if (SwapDamageType == 0)
        {
            Debug.Log("完全命中!");
            Frame -= mobDir * 1f;
            player.Energy +=  100f * 0f;


        }


    }
}
