using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soloMob : MonoBehaviour
{
    soloCenter SoloCenter;
    dirInputSolo dirInput;

    public int mobSort;

    public float mobdir;
    public string mobName;

    public float Hps;
    public float Str;
    public float Rac;//也許有用?
    public float Cur;//無用
    public float Ablock;//無用

    public float ultComsume;

    public float wantToStab = 0f;
    public float addtionalStab = 1f;

    public float wantToUlt = 0f;
    public float addtionalUlt = 1f;

    public float wantToNAttack = 4f;
    public float addtionalNA = 4f;

    public float ABrate = 0.1f;
    public float CBrate = 0.2f;

    SpriteRenderer sr;

    public Sprite normalPic;
    public Sprite attackPic;
    public Sprite defPic;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        SoloCenter = GameObject.Find("SoloCenter").GetComponent<soloCenter>();
        GameObject.Find("SoloCenter").GetComponent<soloCenter>().mob = this;
        dirInput = GameObject.Find("DirInput").GetComponent<dirInputSolo>();
        dirInput.theMob = this;
        transform.parent = GameObject.Find("mobMovement").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeToAttackPic()
    {
        sr.sprite = attackPic;
    }
    public void changeToNormalPic()
    {
        sr.sprite = normalPic;
    }
    public void changeToDefPic()
    {
        sr.sprite = defPic;
    }

    public void mobAttack()
    {
        if (SoloCenter.Frame<(-1* SoloCenter.player.Hps))
        {
            //玩家斬殺
            SoloCenter.Slain();
        }
        else 
        {
            if (wantToUlt > ultComsume)
            {
                mobUlt();
                wantToUlt = 0f;

                wantToStab += addtionalStab;
                wantToNAttack += addtionalNA;
            }
            else if (wantToStab > wantToNAttack)
            {
                mobStab();
                wantToStab = 0f;
                wantToNAttack += addtionalNA;
            }
            else
            {
                mobNormalAttack();

                wantToStab += addtionalStab;
            }

            wantToUlt += addtionalUlt + Random.Range(0f, 50f);
        }
    }

    public virtual float mobDefence(float str)
    {
        float blockRate = Random.Range(0f, 100f);
        blockRate = blockRate / 1000f;
        if (blockRate >= (1f - ABrate))
        {
            //完美格檔
            blockRate = 1f;
            SoloCenter.Frame += 0f;
            SoloCenter.PlayASound();
            Debug.Log(SoloCenter.mob.mobName + "完美格檔");
        }
        else if (blockRate <= CBrate)
        {
            //不完美
            float damageMaked = str * (Random.Range(0f, 100f) / 100f);
            //加料
            SoloCenter.Frame += damageMaked;
            SoloCenter.PlayGoodSound();
            Debug.Log(SoloCenter.mob.mobName + "不完美格檔");
        }
        else
        {
            blockRate = 0f;
            SoloCenter.Frame += str;
            SoloCenter.PlayCSound();
            Debug.Log(SoloCenter.mob.mobName + "格檔失敗");
        }

        return blockRate;
    }
    public virtual void mobDefStab()
    {
        //mob受傷或返回Stab
    }

    public virtual void mobNormalAttack()
    {
        //啟動玩家普通防禦
        mobdir = Random.Range(0f,360f);
        SoloCenter.mobNormalAttack(mobdir);
        
    }
    public virtual void mobSpecialAttack()
    {
        //啟動玩家特殊防禦
        mobNormalAttack();
    }
    public virtual void mobUlt()
    {
        //觸發mob內部攻擊模式
        mobNormalAttack();
    }
    public virtual void mobStab()
    {
        //啟動玩家突刺防禦
        mobNormalAttack();
    }



    //笑死 這段可以廢了 可以直接用prefab改數值就結束 單人模式的開發真的太可愛了 笑開懷
    void sortingMob()
    {
        if (mobSort == 0)
        {
            //測試怪
            mobName = "測試怪人";

            ABrate = 0.1f;
            CBrate = 0.2f;
            
            Hps = 60f;
            Str = 30f;
            
            ultComsume = 300f;

            //攻擊種類傾向
            wantToStab = 0f;
            addtionalStab = 1f;

            wantToUlt = 0f;
            addtionalUlt = 1f;

            wantToNAttack = 4f;
            addtionalNA = 4f;
        }
    }

    public void invokePass(float passtime)
    {
        Invoke("mobAttack", passtime);
    }
}
