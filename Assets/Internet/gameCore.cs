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

    [Networked] public int roundPassed { get; set; }

    //�^�X�k��(�֬����)
    [Networked] public bool turnToPlayer0{ get; set; }

    //�ڦۤv�����W�r! "�����O��"
    //���A���@�ߥHplayer1 ��true��Ĳ�o player2�ۤ�
    [Networked] public bool AttackCall { get; set; }
    [Networked] public bool DefendCall { get; set; }

    //�}�l�I�s
    [Networked] public bool startGameBool { get; set; }

    //�����I�s
    [Networked] public bool endGameBool { get; set; }

    //���a�ƾ� 
    public AudioSource bgmPlayer;
    public AudioSource effectPlayer;
    public circleRounding AttackAllowCircle;
    public bool gameEndBlock;

        //Centrol����
    public GameObject UIpos;
    public GameObject blackFilter;
    public GameObject killLIne0;
    public GameObject killLine1;
    public GameObject FramePos;

    public Text swapFrameShow;

    //�ʵe��(local)



    public Text player0Name;
    public Text player1Name;

    private void Start()
    {
        endGameBool = false;
        gameEndBlock = false;
    }

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
        bgmPlayer.clip = Resources.Load<AudioClip>("combatFile/FastFist_Fighting_common");//Capture BGM
        bgmPlayer.Play();
        //playAnimation
        //GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("�C��... �}�l!\n ���b����GCommonEXEmusic");

        //Start play
        yield return new WaitForSeconds(4f);
        //�p�G�nRandom�֤⪱�a �o��ŧi
        turnToPlayer0 = true;
        startGameBool = true;
        endGameBool = false;
        AttackCall = true;
        yield return null;
    }

    public void hintWordBroadCast(string hintInformation)
    {
        GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint(hintInformation);
    }

    public void UpdatePlayer()
    {
        bool debugBlockPlayer0;//�Ȯɥ��o�� ���L�[�J�[�Ԫ̷|��bug
        if (Object.HasStateAuthority)
        {
            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(0))
            {
                p0 = Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[0]].gameObject.GetComponent<internetPlayer>();
                p0.playerName = p0NameString;
                p0.myPlayerSort = 0;

                debugBlockPlayer0 = true;
            }

            if (Runner.gameObject.GetComponent<gameIinker>().playerIDList.ContainsKey(1))
            {
                p1 = Runner.gameObject.GetComponent<gameIinker>().playerList[Runner.gameObject.GetComponent<gameIinker>().playerIDList[1]].gameObject.GetComponent<internetPlayer>();
                p1.playerName = p1NameString;
                p1.myPlayerSort = 1;
                Debug.Log("�ڪ��D�ڬO1 ����ť�_�Ӧ��I�ǩǪ�");
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
                    p0.AtkCallTired = false;
                    SwapAtkDir = p0.AtkDir;
                    SwapAtkStr = p0.AtkStr;
                    //�ѧ��������쨾�u
                    AttackCall = false;
                    DefendCall = true;

                    p0.AtkCallTired = false;
                    //StartCoroutine(breaker(p0,0));
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

                    //StartCoroutine(breaker(p0, 1));
                    p0.DefCallTired = false;

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
                    //StartCoroutine(breaker(p1, 0));

                    p1.AtkCallTired = false;
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

                    p1.DefCallTired = false;
                    //StartCoroutine(breaker(p1, 1));

                    StartCoroutine(nextRound());
                }
            }
        }


        if (startGameBool)
        {
            Debug.Log("�C���i�椤");
            if (endGameBool)//�C������
            {
                Debug.Log("�C������");
                if (!gameEndBlock)
                {
                    gameEndBlock = true;
                    //���椺�e
                    bgmPlayer.Stop();
                    effectPlayer.Play();
                    blackFilter.SetActive(true);
                }
            }

            if (p0 != null)
            {
                killLIne0.transform.position = new Vector3(-p0.Hps*5.1f, 0f, 0f)+ UIpos.transform.position;
            }
            if (p1 != null)
            {
                killLine1.transform.position = new Vector3(p1.Hps* 5.1f, 0f, 0f) + UIpos.transform.position;
            }

            FramePos.transform.position = new Vector3(Frame * 5.1f, 0f, 0f) + UIpos.transform.position;
        }

        swapFrameShow.text = "�{�b�[���G" + Frame;
    }
    
    IEnumerator nextRound()
    {
        //�p��B�P�B
        float MaxmentRange = SwapAtkDir + SwapCur/2;
        float MinimentRange = SwapAtkDir - SwapCur/2;
        
        float ABlockMaxmentRange = SwapAtkDir + (SwapCur/2 * SwapABloack);
        float ABlockMinimentRange = SwapAtkDir - (SwapCur/2*SwapABloack);



        //��Ǯ��ɧP�_
        if (ABlockMaxmentRange >= SwapDefDir && SwapDefDir >= ABlockMinimentRange)
        {
            SwapDamageType = 2;
        }
        else if (ABlockMaxmentRange > 360f)
        {
            if (SwapDefDir <= (ABlockMaxmentRange - 360f))
            {
                SwapDamageType = 2;
            }
        }
        else if (ABlockMinimentRange < 0f)
        {
            if ((ABlockMinimentRange + 360f) <= SwapDefDir)
            {
                SwapDamageType = 2;
            }
        }

        //���q���ɧP�_
        else if (MaxmentRange >= SwapDefDir && SwapDefDir >= MinimentRange)
        {
            SwapDamageType = 1;
        }
        else if (MaxmentRange > 360f)
        {
            if (SwapDefDir <= (MaxmentRange - 360f))
            {
                SwapDamageType = 1;
            }
        }
        else if (MinimentRange < 0f)
        {
            if ((MinimentRange + 360f) <= SwapDefDir)
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
            if (MaxmentRange > 360f && MinimentRange > SwapDefDir)
            {
                SwapDefDir += 360f;
            }
            else if (MinimentRange < 0f && SwapDefDir > MaxmentRange) 
            {
                SwapDefDir -= 360f;
            }
            float AcureatPercent = Mathf.Abs(SwapAtkDir - SwapDefDir)*(1/SwapCur);
            Debug.Log("���ɺ�ǲv�G"+AcureatPercent);

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
                endGameBool = true;
            }
            Debug.Log("P1����F"+SwapFrameChange+"�I�[���ˮ`!");
            Frame += SwapFrameChange;
            p1.Energy += SwapEnergyRecover;
        }
        else//���a1����
        {
            if (Mathf.Abs(Frame) > p0.Hps)
            {
                //�ٱ�P0
                Debug.Log("P0�ٱ�!");
                endGameBool = true;
            }
            Debug.Log("P0����F" + SwapFrameChange + "�I�[���ˮ`!");
            Frame -= SwapFrameChange;
            p0.Energy += SwapEnergyRecover;
        }

        yield return new WaitForSeconds(nextRoundCoolDown);

        //�U�^�X���i
        turnToPlayer0 = !turnToPlayer0;
        AttackCall = true;
        yield return null;
    }

    IEnumerator breaker(internetPlayer player,int kind)
    {
        yield return new WaitForSeconds(0.1f);
        if (kind == 0)
        {
            player.AtkCallTired = false;
        }
        else
        {
            player.DefCallTired = false;
        }
    }
}
