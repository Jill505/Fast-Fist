using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soloStageManager : MonoBehaviour
{
    public int stageNumber;
    public soloCenter center;

    public int stageLevel;//���d���h��

    public Text mobNameText;

    public void Awake()
    {
        //���stageNumber

        StartGame();
    }

    public void StartGame()
    {

        if (stageNumber == 0)
        {
            //����� ���d�ĴX�h���b�o��
            //LEVEL0
            GameObject mob = Resources.Load<GameObject>("mob/mob001");
            Instantiate( mob, transform.position,transform.rotation);
            Debug.Log(mob.name+"cpatured");
            mob.GetComponent<soloMob>().mobSort = 0;
            mobNameText.text = mob.GetComponent<soloMob>().mobName;
        }
    }
}
