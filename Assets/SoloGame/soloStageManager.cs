using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soloStageManager : MonoBehaviour
{
    public int stageNumber;
    public soloCenter center;
    public SpriteRenderer backgroundImage;

    public int stageLevel;//���d���h��

    public Text mobNameText;

    public void Awake()
    {
        //���stageNumber

        StartGame();
    }

    public void StartGame()
    {
        stageNumber = Random.Range(0, 2);


        if (stageNumber == 0)
        {
            //����� ���d�ĴX�h���b�o��
            //LEVEL0
            GameObject mob = Resources.Load<GameObject>("mob/mob001");
            Instantiate( mob, transform.position,transform.rotation);
            Debug.Log(mob.name+"cpatured");
            mob.GetComponent<soloMob>().mobSort = 0;
            mobNameText.text = mob.GetComponent<soloMob>().mobName;
            backgroundImage.sprite = Resources.Load<Sprite>("background/background001");
        }
        else if (stageNumber == 1)
        {
            GameObject mob = Resources.Load<GameObject>("mob/mob002");
            Instantiate(mob, transform.position, transform.rotation);
            Debug.Log(mob.name + "cpatured");
            mob.GetComponent<soloMob>().mobSort = 0;
            mobNameText.text = mob.GetComponent<soloMob>().mobName;
            backgroundImage.sprite = Resources.Load<Sprite>("background/background001");
        }
    }
}
