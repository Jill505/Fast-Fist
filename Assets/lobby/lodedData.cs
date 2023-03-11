using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class lodedData : MonoBehaviour
{
    [SerializeField] GameObject playerName;
    private void Awake()
    {
        Debug.Log(PlayerPrefs.GetInt("isFirstTimeLoginLocal"));
        localDataBase.localDataJsonDepack();
    }
    void Start()
    {
        dataUpdate();
    }

    public void dataUpdate()
    {
        playerName.GetComponent<Text>().text = localDataBase.PlayerData.Name;
    }

}
