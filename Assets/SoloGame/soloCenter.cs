using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Animator theFist;
    public Animator mobMovement;

    public AudioSource AudioPlayer;
    public AudioClip AcuracyHitSound;
    public AudioClip GoodHitSound;
    public AudioClip CompleatsHitSound;
    public AudioClip SlainSound;
    public AudioClip StabSound;

    public GameObject theFramePos;
    public GameObject midPoint;
    public GameObject PlayerPos;
    public GameObject MobPos;

    public GameObject GameEndVail;
    public Text EndWord;


    public GameObject Mob;
    public Animator MobAnimator;


    public float Frame;
    /*{ get { return frame; } 
        set
        { //update
            frame += value;
        } 
    
    }*/

    public int stageSort;//���d�ǦC

    public Text mobNameText;

    public float mobAttackDir;

    public void Slain()
    {
        GameEndVail.SetActive(true);
        AudioPlayer.clip = SlainSound;
        AudioPlayer.Play();
    }

    public IEnumerator gameStart()
    {
        if (stageSort == 0)
        {
            //�����
            //mob.mobSort = 0;
            //mobNameText.text = mob.mobName;
        }

        gameStarted = true;
        DirInputSolo.allowInputAtk = true;
        //atkHintRing.SetTrigger("show");
        //�}�l���񭵼�
        yield return null;

        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        //�}���ʵe
        StartCoroutine(gameStart());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("YA"+ theFramePos.transform.position);
        theFramePos.transform.position = new Vector3(Frame * 3.3f, 0f, 0f) + midPoint.transform.position;
        PlayerPos.transform.position = new Vector3(player.Hps*-3.3f, 0f, 0f) + midPoint.transform.position; 
        MobPos.transform.position = new Vector3(mob.Hps*3.3f, 0f, 0f) + midPoint.transform.position;



        if (gameStarted)
        {
            if (turnToPlayer)
            {
                if (!casting)
                {
                    //�����a���(�����Ω�j)
                    casting = true;
                    DirInputSolo.allowInputAtk = true;
                    //��ܧ�����
                    atkHintRing.SetTrigger("show");
                }
                //����
            }
            else
            {
                if (!casting)
                {
                    //mob���
                    casting = true;
                    mob.mobAttack();
                }
                //����
            }
        }
    }
    private void LateUpdate()
    {
        //��ܬ[��
    }


    //�Ǫ����q�������f
    public void mobNormalAttack(float dir)
    {
        mobAttackDir = dir;
        DirInputSolo.allowInputDef= true;
        Fist.transform.localEulerAngles = new Vector3(0,0,dir);

        defHintFist.SetTrigger("show");
        theFist.SetTrigger("punchedDown");
        //Animation Active
        StartCoroutine(touch());
    }
    public void mobStab(float area)
    {

    }

    public void playerRecat()
    {

    }

    public void playerReactMobNormalAttack(float playerReturnDir, float mobDir)
    {
        Debug.Log(playerReturnDir + "�㺿" + mobDir);


        defHintFist.SetTrigger("hide");

        //�p��B�P�B
        float MaxmentRange = mobDir + player.Cur / 2;
        float MinimentRange = mobDir - player.Cur / 2;

        float ABlockMaxmentRange = mobDir + (player.Cur / 2 * player.Ablock);
        float ABlockMinimentRange = mobDir - (player.Cur / 2 * player.Ablock);

        int SwapDamageType = 0;
        
        //�ٱ��b�o
        if (mob.Hps < Frame)
        {
            //�ٱ�
            Slain();
        }
        else
        {
            //��Ǯ��ɧP�_
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

            //���q���ɧP�_
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
            //�������
            else
            {
                SwapDamageType = 0;
            }

        
            //���˧P�_
            if (SwapDamageType == 2)
            {
                Debug.Log("��T����!");
                Frame -= mob.Str * 0f;
                player.Energy += 100f * 1f;

                PlayASound();
            }
            else if (SwapDamageType == 1)
            {
                Debug.Log("��������");
                //��ǫ׭p��
                if (MaxmentRange > 360f && MinimentRange > playerReturnDir)
                {
                    playerReturnDir += 360f;
                }
                else if (MinimentRange < 0f && playerReturnDir > MaxmentRange)
                {
                    playerReturnDir -= 360f;
                }
                float AcureatPercent = Mathf.Abs(mobDir - playerReturnDir) * (1 / player.Cur);
                Debug.Log("���ɺ�ǲv�G" + AcureatPercent);

                Frame -= mob.Str * (1f - AcureatPercent);
                player.Energy += 100f * AcureatPercent;

                PlayGoodSound();
            }
            else if (SwapDamageType == 0)
            {
                Debug.Log("�����R��!");
                Frame -= mob.Str * 1f;
                player.Energy += 100f * 0f;

                PlayCSound();
            }
        }
    }

    public void PlayASound()
    {
        AudioPlayer.clip = AcuracyHitSound;
        AudioPlayer.Play();
    }

    public void PlayGoodSound()
    {
        AudioPlayer.clip = GoodHitSound;
        AudioPlayer.Play();
    }

    public void PlayCSound()
    {
        AudioPlayer.clip = CompleatsHitSound;
        AudioPlayer.Play();
    }

    public void NextRoundJudgement()
    {
        turnToPlayer = !turnToPlayer;

        casting = false;
    }

    public void BackToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public IEnumerator touch()
    {
        yield return new WaitForSeconds(0.36f);
        theFist.speed = 0.1f;

        //Mob�ʵe
        mobMovement.SetBool("Attack",true);
        //mob.changeToAttackPic();
        yield return new WaitForSeconds(player.Rac - 36f);
        //casting �j���
        mobMovement.SetBool("Attack", false);
        //mob.changeToNormalPic();
        theFist.speed = 1f;
        playerReactMobNormalAttack(55688f, DirInputSolo.theMob.mobdir);
        DirInputSolo.allowInputDef = !DirInputSolo.allowInputDef;

        NextRoundJudgement();
    }
}
