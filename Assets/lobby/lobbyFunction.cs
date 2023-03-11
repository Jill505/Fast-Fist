using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class lobbyFunction : MonoBehaviour
{
    [SerializeField] InputField sessionInput;
    public void startGameInternet()
    {
        string sessionName = sessionInput.text;
        Debug.Log(sessionName);
        SceneManager.LoadScene("combat");
    }
}
