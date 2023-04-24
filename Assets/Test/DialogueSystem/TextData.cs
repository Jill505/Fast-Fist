using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour
{
    [System.Serializable]
    public class character
    {
        public string name; //角色名稱
        public Sprite characterImage; //角色的圖片
    }

    public character[] characterArray; 

    [System.Serializable]
    public class dialogueContent
    {
        public int characterNumber; //正在說話的角色
        public string dialogueText; //對話的文字
    }

    public dialogueContent[] dialogueContentArray; 
}
