using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soloPlayer : MonoBehaviour
{
    soloCenter center;
    dirInputSolo dirinput;

    public Button skillButton;
    public Image buttonButton;
    public Image buttonFilliter;
    public AudioSource AS;

    //玩家基礎數據
    public float Hps;
    public float Str;
    public float Rac;
    public float Cur;
    public float Ablock;


    //現存玩家數據
    public float AtkTime;
    public float Energy;
    public float UltComsume;
    public float MaxmentEnergy;

    public int myCharacterSort;

    private void Awake()
    {
        center = GameObject.Find("SoloCenter").GetComponent<soloCenter>();
        center.player = this;
        dirinput = GameObject.Find("DirInput").GetComponent<dirInputSolo>();
        myCharacterSort = localDataBase.PlayerData.selectionCharacter;

        //賦值
        sortingPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (center.gameStarted)//遊戲開始
        {
            /*
            if (center.turnToPlayer == true)//輪到自己回合
            {
                //攻擊
                dirinput.allowInputAtk = true;
                //顯示攻擊圈
            }
            else
            {
                //防守
                dirinput.allowInputDef = true;
            }*/
        }

        if (Energy >= MaxmentEnergy)
        {
            Energy = MaxmentEnergy;
        }

        if (Energy >= UltComsume)
        {
            skillButton.interactable = true;
            Debug.Log("技能處於可開啟期間");
        }
        else
        {
            skillButton.interactable = false;
        }

        buttonFilliter.fillAmount = Energy / UltComsume;
    }

    void sortingPlayer()
    {
        if (localDataBase.PlayerData.selectionCharacter == 0)
        {
            Hps = 60f;
            Str = 20f;
            Rac = 100f;
            Cur = 90f;
            Ablock = 0.1f;

            AtkTime = 5f;

            Energy = 0f;
            UltComsume = 150f;
            MaxmentEnergy = 150f;

            buttonButton.sprite = Resources.Load<Sprite>("combatFile/Skill0Image");
            buttonFilliter.sprite = Resources.Load<Sprite>("combatFile/Skill0Image");
        }
    }

    public void useSkill()
    {
        Energy -= UltComsume;

        if (myCharacterSort == 0)
        {
            AS.clip = Resources.Load<AudioClip>("combatFile/skillSoundEffect/MD");
            AS.Play();
            StartCoroutine(MDmode());
        }
    }

    IEnumerator MDmode()
    {
        Str += 30f;
        yield return new WaitForSeconds(10f);
        Str -= 30f;
        yield return null;
    }
}
