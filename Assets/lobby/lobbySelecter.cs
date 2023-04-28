using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lobbySelecter : MonoBehaviour
{
    public Animator lobbyAnimator;

    public Animator Bt0;//Shop
    public bool bt0B;
    public Animator Bt1;//lobby
    public bool bt1B;
    public Animator Bt2;//Char
    public bool bt2B;

    private void Start()
    {
        Bt1.SetTrigger("selecte");
        bt1B = true;
    }

    public void switchToLobby()
    {
        Bt1.SetTrigger("selecte");
        bt1B = true;
        if (bt0B)
        {
            bt0B = !bt0B;
            Bt0.SetTrigger("cancel");
            lobbyAnimator.SetTrigger("shopToLobby");

        }
        if (bt2B)
        {
            bt2B = !bt2B;
            Bt2.SetTrigger("cancel");
            lobbyAnimator.SetTrigger("charToLobby");
        }
    }

    public void switchToCharacterSelection()
    {
        bt2B = true;
        Bt2.SetTrigger("selecte");

        if (bt0B)
        {
            bt0B = !bt0B;
            Bt0.SetTrigger("cancel");
            lobbyAnimator.SetTrigger("shopToChar");
        }
        if (bt1B)
        {
            bt1B = !bt1B;
            Bt1.SetTrigger("cancel");
            lobbyAnimator.SetTrigger("lobbyToChar");
        }
    }

    public void switchToShop()
    {
        bt0B = true;
        Bt0.SetTrigger("selecte");

        if (bt1B)
        {
            bt1B = !bt1B;
            Bt1.SetTrigger("cancel");
            lobbyAnimator.SetTrigger("lobbyToShop");
        }
        if (bt2B)
        {
            bt2B = !bt2B;
            Bt2.SetTrigger("cancel");
            lobbyAnimator.SetTrigger("charToShop");
        }
    }
}
