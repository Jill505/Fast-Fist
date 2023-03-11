using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerData
{
    //基礎帳號數據
    public float gold;
    public float diamond;
    public string Name;//玩家名字

    //基礎本地數據
    public int selectionMoodsort;//選擇中心情
    public int selectionCharacter;//選擇中角色

    //底下寫持有的特殊item與數量
    public int[] item;
    //底下寫持有腳色
    public bool[] haveCharacter;
    //底下寫持有心情
    public bool[] haveMood;

    //底下寫特殊資料 以後用新的自訂類別
    public bool[] loginTimes;//登陸次數
}
