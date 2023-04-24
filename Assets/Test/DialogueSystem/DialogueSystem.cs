using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public TextData textData;

    public Image characterImage;
    public Text characterName; //Text�ե�:����W��
    public Text dialogueText; //Text�ե�:��ܤ�r

    public bool isInputSkip = false;

    public float typingSpeed;

    public void StartDialogue() // �}�l���
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

            //�v�r��ܹ�ܤ奻
            foreach (char c in textData.dialogueContentArray[i].dialogueText)
            {
                dialogueText.text += c;

                if(!Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Space))
                {
                    isInputSkip = false;
                }

                if (Input.GetKey(KeyCode.Escape)) //���L��q���
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

            while (!Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Space)) //���ݫ��U���� �i�J�U�@�q���
            {
                if (Input.GetKeyDown(KeyCode.Escape)) //���L��q���
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

    public void EndDialogue() //�������
    {

        Destroy(gameObject);
    }
}
