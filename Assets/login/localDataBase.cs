using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localDataBase : MonoBehaviour
{
    //public int isFirstTimeLoginLocal;//此玩家是否是第一次進入遊客登入 0/是 1/否
    //static public float gold;

    static public playerData PlayerData = new playerData();

    public void getSleepingFile()
    {
        if (PlayerPrefs.GetInt("isFirstTimeLoginLocal") == 0)//第一次登入本地
        {
            //建立本地檔案+初始化
            backToPast();

            //playerData Swaper;
        }
        else
        {
            //載入本地檔案
            localDataJsonDepack();
        }
    }

    public void firstTime_QuestionMark()//確認帳號狀態
    {
        //if()
    }

    public void backToPast()//初始化
    {
        PlayerPrefs.SetInt("isFirstTimeLoginLocal", 1);

        PlayerPrefs.SetFloat("gold", 0f);
        PlayerPrefs.SetFloat("Float", 0f);
    }

    static public void localDataJsonPack()//儲存
    {
        Debug.Log(PlayerPrefs.GetString("PlayerDataLocal"));
        PlayerPrefs.SetString("PlayerDataLocal", JsonUtility.ToJson(PlayerData));
        Debug.Log("儲存完畢");
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("PlayerDataLocal"));
    }

    static public void localDataJsonDepack()//解包 必須和和playerData裡面的資料保持一致
    {
        var Swaper = JsonUtility.FromJson<playerData>(PlayerPrefs.GetString("PlayerDataLocal"));
        Debug.Log(PlayerPrefs.GetString("PlayerDataLocal"));
        Debug.Log("成功解包本地玩家資料"+ "print="+ PlayerData.Name +"=分隔大師="+ Swaper.Name);
        PlayerData.diamond = Swaper.diamond;
        PlayerData.gold = Swaper.gold;

        PlayerData.selectionCharacter = Swaper.selectionCharacter;
        PlayerData.selectionMoodsort = Swaper.selectionMoodsort;

        PlayerData.Name = Swaper.Name;

        PlayerData.loginTimes = Swaper.loginTimes;
    }
}
