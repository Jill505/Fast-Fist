using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct playerInputData : INetworkInput
{
    //public Vector3 movementInput;
    //public playerInputData playerInformationInput;
    public int moodSelectionSort;
    public int characterSelectionSort;

}

public struct PlayerInformation : INetworkInput
{
    public float Hps;
    public float Str;
    public float Rac;
    public float Cur;
    public float Ablock;

    public float MaxmentEnergy;
    public float Energy;
}

public struct AtkInputData : INetworkInput
{
    public bool AtkFinished;//完成呼叫

    public float AtkStr;//攻擊力

    public float AtkDir;//攻擊角度
}

public struct DefInputData : INetworkInput
{
    public bool DefFinished;//完成呼叫

    //public float FrameChanged;//架式改變數據

    public float DefDir;//防守角度角度 用於同步防守動畫
    //public float DefCur;//防守格檔角度
    //public float ABloack;//精準格檔角度 Accurate Block

    //public int injuredType; //受傷種類 用於同步雙方動畫播放類型 0 = 未受傷; 1 = 受傷一半; 2 = 完整受傷;
}

public struct SpecialData : INetworkInput
{
    public float silent;//沉默申請
    public float moreRoundRequest;//更多回合數 
}

public struct TestStruck : INetworkInput
{
    public int uploadInt;
}

public struct DebugSpace : INetworkInput
{
    public float debugAtkDir;
    public float debugDefDir;
}