using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleRounding : MonoBehaviour
{
    [SerializeField] [Range(-10f, 10f)] float rotatingSpeed;
    [SerializeField] Animator daddy;
    private void FixedUpdate()
    {
        transform.Rotate(0f,0f,1f*rotatingSpeed);
    }

    public void showMe()
    {
        daddy.SetTrigger("show");
    }

    public void hideMe()
    {
        daddy.SetTrigger("hide");
    }
}
