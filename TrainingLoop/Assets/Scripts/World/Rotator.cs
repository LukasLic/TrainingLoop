using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool Clockwise = true;
    public float Speed = 2f;

    private void Update()
    {
        if (Clockwise)
            transform.Rotate(-transform.forward, Speed * Time.deltaTime);
        else
            transform.Rotate(transform.forward, Speed * Time.deltaTime);
    }
}
