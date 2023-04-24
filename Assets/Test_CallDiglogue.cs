using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CallDiglogue : MonoBehaviour
{
    public DialogueSystem dialogueSystem;

    void Start()
    {
        dialogueSystem.StartDialogue();
    }
}
