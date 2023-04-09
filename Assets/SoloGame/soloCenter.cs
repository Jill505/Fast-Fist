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

    public int stageSort;//���d�ǦC

    public Text mobNameText;

    public float mobAttackDir;

    public IEnumerator gameStart()
    {
        if (stageSort == 0)
        {
            //�����
            //mob.mobSort = 0;
            //mobNameText.text = mob.mobName;
        }

        gameStarted = true;
        //�}�l���񭵼�
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
        DirInputSolo.allowInputDef= true;
        Fist.transform.localEulerAngles = new Vector3(0,0,dir);
        defHintFist.SetTrigger("show");
        //Animation Active
    }

    public void playerReactMobNormalAttack(float playerReturnDir, float mobDir)
    {
        defHintFist.SetTrigger("hide");

        //�p��B�P�B
        float MaxmentRange = mobDir + player.Cur / 2;
        float MinimentRange = mobDir - player.Cur / 2;

        float ABlockMaxmentRange = mobDir + (player.Cur / 2 * player.Ablock);
        float ABlockMinimentRange = mobDir - (player.Cur / 2 * player.Ablock);

        int SwapDamageType = 0;

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
            Frame -= mobDir * 0f;
            player.Energy += 100f * 1f;


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

            Frame -= mobDir * (1f - AcureatPercent);
            player.Energy += 100f * AcureatPercent;


        }
        else if (SwapDamageType == 0)
        {
            Debug.Log("�����R��!");
            Frame -= mobDir * 1f;
            player.Energy +=  100f * 0f;


        }


    }
}
