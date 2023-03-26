using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class gameCore : NetworkBehaviour
{
    [Networked] public int numberIntheScene { get; set; }

    [Networked] public string p0NameString { get; set; }
    [Networked] public string p1NameString { get; set; }

    public internetPlayer p0;
    public internetPlayer p1;

    //�@�μƾ�
    [Networked(OnChanged = nameof(changeFrame))] public float Frame { get; set; }//�[��
    [Networked] public float nextRoundCoolDown { get; set; }

    [Networked] public float SwapAtkStr { get; set; }
    [Networked] public float SwapAtkDir { get; set; }
    [Networked] public float SwapDefDir { get; set; }
    [Networked] public float SwapABloack { get; set; }
    [Networked] public float SwapCur { get; set; }

    [Networked] public int SwapDamageType { get; set; }//0=�S�� 1=�������� 2=��Ǯ���

    [Networked] public float SwapFrameChange { get; set; }

    [Networked] public float SwapEnergyRecover { get; set; }

    //�^�X�k��(�֬����)
    [Networked] public bool turnToPlayer0{ get; set; }

    //�ڦۤv�����W�r! "�����O��"
    //���A���@�ߥHplayer1 ��true��Ĳ�o player2�ۤ�
    [Networked] public bool AttackCall { get; set; }
    [Networked] public bool DefendCall { get; set; }

    //�}�l�I�s
    [Networked] public bool startGameBool { get; set; }

    //���a�ƾ�
    public AudioSource bgmPlayer;
    public circleRounding AttackAllowCircle;

    public Text swapFrameShow;

    //�ʵe��(local)



    public Text player0Name;
    public Text player1Name;

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_namePlayer()
    {
        if (p0 != null)
        {
             player0Name.text = "P0";
            //player0Name.text = p0.playerName;
        }
        if (p1 != null)
        {
            player1Name.text = "P1";
            //player0Name.text = p1.playerName;
        }
    }

    public static void changeFrame(Changed<gameCore> changed)
    {
        changed.Behaviour.swapFrameShow.text = "�[���G"+ changed.Behaviour.Frame;
    }

    public void reStartGame()
    {
        Frame = 0f;
        StartCoroutine(startGameIEnumerator());
    }

    IEnumerator startGameIEnumerator()
    {
        yield return new WaitForSeconds(3f);
        //�ƾڦP�B
        nextRoundCoolDown = 0.6f;
        //playMusic
        bgmPlayer.clip = Resources.Load<AudioClip>("DelFile/CommonEXEmusic");//Capture BGM
        bgmPlayer.Play();
        //playAnimation
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("�C��... �}�l!\n ���b����GCommonEXEmusic");

        //Start play
        yield return new WaitForSeconds(4f);
        //�p�G�nRandom�֤⪱�a �o��ŧi
        turnToPlayer0 = true;
        startGameBool = true;
        AttackCall = true;
        yield return null;
    }

    public void hintWordBroadCast(string hintInformation)
    {
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint(hintInformation);
    }

    public void UpdatePlayer()
    {
        if (Object.HasStateAuthority)
        {
            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(0))
            {
                p0 = Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>();
                p0.playerName = p0NameString;
                p0.myPlayerSort = 0;
            }

            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(1))
            {
                p1 = Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>();
                p1.playerName = p1NameString;
                p1.myPlayerSort = 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void FixedUpdateNetwork()
    {
        //capture player's information
        if (Object.HasStateAuthority)
        {
            if (p0 != null)
            {
                //�P�B
                if (p0.AtkFinishedCall)
                {
                    p0.AtkFinishedCall = false;
                    SwapAtkDir = p0.AtkDir;
                    SwapAtkStr = p0.AtkStr;
                    //�ѧ��������쨾�u
                    AttackCall = false;
                    DefendCall = true;
                }
                if (p0.DefFinishedCall)
                {
                    //���u�쵲��
                    p0.DefFinishedCall = false;
                    SwapDefDir = p0.DefDir;
                    SwapABloack = p0.Ablock;
                    SwapCur = p0.Cur;
                    DefendCall = false;
                    //�^�X����
                    StartCoroutine(nextRound());
                }
            }

            if (p1 != null)
            {
                //�P�B
                if (p1.AtkFinishedCall)
                {
                    p1.AtkFinishedCall = false;
                    SwapAtkDir = p1.AtkDir;
                    SwapAtkStr = p1.AtkStr;
                    //�ѧ��������쨾�u
                    AttackCall = false;
                    DefendCall = true;
                }
                if (p1.DefFinishedCall)
                {
                    //���u�쵲��
                    p1.DefFinishedCall = false;
                    SwapDefDir = p1.DefDir;
                    SwapABloack = p1.Ablock;
                    SwapCur = p1.Cur;
                    DefendCall = false;
                    //�^�X����
                    StartCoroutine(nextRound());
                }
            }
        }
    }
    
    IEnumerator nextRound()
    {
        //�p��B�P�B
        float MaxmentRange = SwapAtkDir + SwapCur;
        float MinimentRange = SwapAtkDir - SwapCur;
        
        float ABlockMaxmentRange = SwapAtkDir + (SwapCur * SwapABloack);
        float ABlockMinimentRange = SwapAtkDir - (SwapCur*SwapABloack);



        //��Ǯ��ɧP�_
        if (ABlockMaxmentRange >= SwapDefDir || SwapDefDir >= ABlockMinimentRange)
        {
            SwapDamageType = 2;
        }
        else if (ABlockMaxmentRange > 360f)
        {
            if (SwapDefDir >= (ABlockMaxmentRange % 360f))
            {
                SwapDamageType = 2;
            }
        }
        else if (ABlockMinimentRange < 0f)
        {
            if ((ABlockMinimentRange + 360f) >= SwapDefDir)
            {
                SwapDamageType = 2;
            }
        }
        //���q���ɧP�_
        else if (MaxmentRange >= SwapDefDir || SwapDefDir >= MinimentRange)
        {
            SwapDamageType = 1;
        }
        else if (MaxmentRange > 360f)
        {
            if (SwapDefDir >= (MaxmentRange % 360f))
            {
                SwapDamageType = 1;
            }
        }
        else if (MinimentRange < 0f)
        {
            if ((MinimentRange + 360f) >= SwapDefDir)
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
            SwapFrameChange = SwapAtkStr * 0f;
            SwapEnergyRecover = 100f*1f;
        }
        else if (SwapDamageType == 1)
        {
            Debug.Log("��������");
            //��ǫ׭p��
            float AcureatPercent = 0.5f;

            SwapFrameChange = SwapAtkStr * (1f - AcureatPercent);
            SwapEnergyRecover = 100f * AcureatPercent;
        }
        else if (SwapDamageType == 0)
        {
            Debug.Log("�����R��!");
            SwapFrameChange = SwapAtkStr * 1f;
            SwapEnergyRecover = 100f*0f;
        }

        //�}�l���˻P�^�X�P�_
        if (turnToPlayer0)//���a0����
        {
            if (Mathf.Abs(Frame) > p1.Hps)
            {
                //�ٱ�P1
                Debug.Log("P1�ٱ�!");
            }
            Frame += SwapFrameChange;
        }
        else//���a1����
        {
            if (Mathf.Abs(Frame) > p0.Hps)
            {
                //�ٱ�P0
                Debug.Log("P0�ٱ�!");
            }
            Frame -= SwapFrameChange;
        }

        yield return new WaitForSeconds(nextRoundCoolDown);

        //�U�^�X���i
        turnToPlayer0 = !turnToPlayer0;
        AttackCall = true;
        yield return null;
    }
}
