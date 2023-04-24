using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour
{
    [System.Serializable]
    public class character
    {
        public string name; //����W��
        public Sprite characterImage; //���⪺�Ϥ�
    }

    public character[] characterArray; 

    [System.Serializable]
    public class dialogueContent
    {
        public int characterNumber; //���b���ܪ�����
        public string dialogueText; //��ܪ���r
    }

    public dialogueContent[] dialogueContentArray; 
}
