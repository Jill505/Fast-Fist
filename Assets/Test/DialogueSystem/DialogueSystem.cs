using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public TextData textData;

    public Image characterImage;
    public Text characterName; //Text組件:角色名稱
    public Text dialogueText; //Text組件:對話文字

    public bool isInputSkip = false;

    public float typingSpeed;

    public void StartDialogue() // 開始對話
    {
        gameObject.SetActive(true);
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for(int i = 0 ; i < textData.dialogueContentArray.Length; )
        {
            characterName.text = textData.characterArray[textData.dialogueContentArray[i].characterNumber].name;
            characterImage.sprite = textData.characterArray[textData.dialogueContentArray[i].characterNumber].characterImage;

            //逐字顯示對話文本
            foreach (char c in textData.dialogueContentArray[i].dialogueText)
            {
                dialogueText.text += c;

                if(!Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Space))
                {
                    isInputSkip = false;
                }

                if (Input.GetKey(KeyCode.Escape)) //跳過整段對話
                {
                    EndDialogue();
                }

                if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space)) && isInputSkip == false)
                {
                    dialogueText.text = textData.dialogueContentArray[i].dialogueText;
                    break;
                }

                yield return new WaitForSeconds(typingSpeed);
               
            }

            while (!Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Space)) //等待按下按鍵 進入下一段對話
            {
                if (Input.GetKeyDown(KeyCode.Escape)) //跳過整段對話
                {
                    EndDialogue();
                }

                yield return null;
            }

            isInputSkip = true;
            i++;

            dialogueText.text = "";

        }

        EndDialogue();
    }

    public void EndDialogue() //結束對話
    {

        Destroy(gameObject);
    }
}
