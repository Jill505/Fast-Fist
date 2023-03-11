using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hintWord : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void startHint(string hintString)
    {
        gameObject.GetComponent<Text>().text = hintString;
        animator.SetTrigger("hint");
    }
}
