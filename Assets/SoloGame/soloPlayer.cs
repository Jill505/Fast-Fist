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

    //���a��¦�ƾ�
    public float Hps;
    public float Str;
    public float Rac;
    public float Cur;
    public float Ablock;


    //�{�s���a�ƾ�
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

        //���
        sortingPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (center.gameStarted)//�C���}�l
        {
            /*
            if (center.turnToPlayer == true)//����ۤv�^�X
            {
                //����
                dirinput.allowInputAtk = true;
                //��ܧ�����
            }
            else
            {
                //���u
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
            Debug.Log("�ޯ�B��i�}�Ҵ���");
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
