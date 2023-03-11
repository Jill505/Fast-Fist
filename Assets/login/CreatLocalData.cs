using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreatLocalData : MonoBehaviour
{
    public GameObject CreatLocalCanvas;
    public InputField targetInputField;
    public void CreatOrLogin()
    {
        if (PlayerPrefs.GetInt("isFirstTimeLoginLocal") == 1)
        {
            localDataBase.localDataJsonDepack();
            SceneManager.LoadScene("Lobby");
        }
        else//�гy�ɮ׭���
        {
            GameObject.Find("universalHintWord").GetComponent<hintWord>().startHint("���a�ݥ������");
            CreatLocalCanvas.SetActive(true);
        }
    }

    public void exchangeName()
    {
        CreatLocalCanvas.SetActive(true);
    }

    public void SubmitName()
    {
        localDataBase.PlayerData.Name = targetInputField.text;
        localDataBase.localDataJsonPack();
        PlayerPrefs.SetInt("isFirstTimeLoginLocal", 1);
        CreatLocalCanvas.SetActive(false);
    }
}