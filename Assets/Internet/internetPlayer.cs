using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class internetPlayer : NetworkBehaviour
{
    [SerializeField]
    public gameCore gameCores;

    public int myPlayerSort;

    public DirInput dirInput;

   [Networked]
   public string playerName { get; set; }

    [Networked]
    public int moodSelectionSort { get; set; }
    [Networked]
    public int characterSelectionSort { get; set; }


    //玩家基礎數據
    [Networked]
    public float Hps { get; set; }
    [Networked]
    public float Str { get; set; }
    [Networked]
    public float Rac { get; set; }
    [Networked]
    public float Cur { get; set; }
    [Networked]
    public float Ablock { get; set; }

    //玩家
    [Networked] float AtkDir { get; set; }
    [Networked] float DefDir { get; set; }
    [Networked] float FrameChanged { get; set; }


    //測試
    [Networked]
    public int TestVariable { get; set; }

    //本地
    public bool AtkCallTired;
    public bool DefCallTired;

    private void Awake()
    {
        gameCores = GameObject.Find("gameCore").GetComponent<gameCore>();
        dirInput = GameObject.Find("DirInputSystem").GetComponent<DirInput>();
        //playerName = localDataBase.PlayerData.Name;
    }

    public PlayerRef myLocalPlayer()
    {
        return Runner.LocalPlayer;
    }

    public override void FixedUpdateNetwork()
    {

        //playerName = localDataBase.PlayerData.Name;
        gameCores.Rpc_namePlayer();


        //同步網路數據
        bool swapBool;
        swapBool = gameCores.turnToPlayer0;
        swapBool = gameCores.AttackCall;
        swapBool = gameCores.DefendCall;

        if (GetInput(out TestStruck data))
        {
            TestVariable += data.uploadInt;
            if (myPlayerSort != 0)
            {
                TestVariable -= 2 * data.uploadInt;
            }
        }

        if (GetInput(out playerInputData data0))
        {
            characterSelectionSort = data0.characterSelectionSort;
            moodSelectionSort = data0.moodSelectionSort;
            playerName = localDataBase.PlayerData.Name;

            CharacterGivingValue(characterSelectionSort);

            myPlayerSort = gameCores.numberIntheScene;

            Runner.gameObject.GetComponent<gameIinker>().brocastedName = true;
        }

        if (GetInput(out AtkInputData data1))
        {

        }


        if (Object.HasInputAuthority)
        {
            if (gameCores.startGameBool)
            {
                if (myPlayerSort == 0)
                {
                    if (gameCores.turnToPlayer0 == true)
                    {
                        if (gameCores.AttackCall == true)
                        {
                            if (!AtkCallTired)
                            {
                                AtkCallTired = true;
                                StartCoroutine(Atk());
                            }
                        }
                    }
                    else
                    {
                        if (gameCores.DefendCall == true)
                        {
                            if (!DefCallTired)
                            {
                                DefCallTired = true;
                                StartCoroutine(Def());
                            }
                        }
                    }
                }
                else if (myPlayerSort == 1)
                {
                    if (gameCores.turnToPlayer0 == false)
                    {
                        if (gameCores.AttackCall == true)
                        {
                            if (!AtkCallTired)
                            {
                                AtkCallTired = true;
                                StartCoroutine(Atk());
                            }
                        }
                    }
                    else
                    {
                        if (gameCores.DefendCall == true)
                        {
                            if (!DefCallTired)
                            {
                                DefCallTired = true;
                                StartCoroutine(Def());
                            }
                        }
                    }
                }
            }
            
        }

    }

    public void CharacterGivingValue(int characterSorting)
    {

        playerName = localDataBase.PlayerData.Name;
        gameCores.Rpc_namePlayer();


        characterSelectionSort = characterSorting;

        if (characterSorting == 0)
        {
            //Defult Character
            Hps = 60f;
            Str = 30f;
            Rac = 90f;
            Cur = 90f;
            Ablock = 0.2f;
        }
        else if (characterSorting == 1)
        {
            //蝦兵
        }
        else if (characterSorting == 2)
        {
            //黑斯
        }
    }


    IEnumerator Atk()
    {

        yield return null;
    }
    void AtkBreak()
    {

    }

    IEnumerator Def()
    {
        yield return null;
    }
    void DefBreak()
    {

    }
}


