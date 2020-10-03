using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConstantForce2D))]
public class WorldGravity : MonoBehaviour
{
    [SerializeField]
    private ConstantForce2D _constantForce;
    public  ConstantForce2D ConstantForce
    {
        get
        {
            if (_constantForce == null)
                _constantForce = GetComponent<ConstantForce2D>();

            return _constantForce;
        }
    }

    public Transform WorldCenter;
    public float gravityStrength = 5f;

    public Vector2 Up
    {
        get
        {
            return (WorldCenter.position - transform.position).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ConstantForce.force = -Up * gravityStrength;
    }
}
