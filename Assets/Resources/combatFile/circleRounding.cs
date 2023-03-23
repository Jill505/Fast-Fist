using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleRounding : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] float rotatingSpeed;
    private void FixedUpdate()
    {
        transform.Rotate(0f,0f,1f*rotatingSpeed);
    }
}
