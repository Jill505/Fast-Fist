using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localDataBase : MonoBehaviour
{
    //public int isFirstTimeLoginLocal;//�����a�O�_�O�Ĥ@���i�J�C�ȵn�J 0/�O 1/�_
    //static public float gold;

    static public playerData PlayerData = new playerData();

    public void getSleepingFile()
    {
        if (PlayerPrefs.GetInt("isFirstTimeLoginLocal") == 0)//�Ĥ@���n�J���a
        {
            //�إߥ��a�ɮ�+��l��
            backToPast();

            //playerData Swaper;
        }
        else
        {
            //���J���a�ɮ�
            localDataJsonDepack();
        }
    }

    public void firstTime_QuestionMark()//�T�{�b�����A
    {
        //if()
    }

    public void backToPast()//��l��
    {
        PlayerPrefs.SetInt("isFirstTimeLoginLocal", 1);

        PlayerPrefs.SetFloat("gold", 0f);
        PlayerPrefs.SetFloat("Float", 0f);
    }

    static public void localDataJsonPack()//�x�s
    {
        Debug.Log(PlayerPrefs.GetString("PlayerDataLocal"));
        PlayerPrefs.SetString("PlayerDataLocal", JsonUtility.ToJson(PlayerData));
        Debug.Log("�x�s����");
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("PlayerDataLocal"));
    }

    static public void localDataJsonDepack()//�ѥ] �����M�MplayerData�̭�����ƫO���@�P
    {
        var Swaper = JsonUtility.FromJson<playerData>(PlayerPrefs.GetString("PlayerDataLocal"));
        Debug.Log(PlayerPrefs.GetString("PlayerDataLocal"));
        Debug.Log("���\�ѥ]���a���a���"+ "print="+ PlayerData.Name +"=���j�j�v="+ Swaper.Name);
        PlayerData.diamond = Swaper.diamond;
        PlayerData.gold = Swaper.gold;

        PlayerData.selectionCharacter = Swaper.selectionCharacter;
        PlayerData.selectionMoodsort = Swaper.selectionMoodsort;

        PlayerData.Name = Swaper.Name;

        PlayerData.loginTimes = Swaper.loginTimes;
    }
}
