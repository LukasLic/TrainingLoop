using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

[ExecuteInEditMode]
public class RotateAtTransform : MonoBehaviour
{
    /// <summary>
    /// Works as a button from editor
    /// </summary>
    public bool Rotate;

    public Transform Target;
    public List<Transform> Objects;

    // Update is called once per frame
    void Update()
    {
        if(Application.isEditor)
        {
            if (Rotate)
            {
                Rotate = false;
                DoRotate();
            }
        }
    }

    public void DoRotate()
    {
        foreach (var o in Objects)
        {
            //o.LookAt(Target);

            var dir = Target.position - o.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
            o.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //o.Rotate(o.transform.up, 90f);

            //Vector3 targetPostition = new Vector3(o.transform.position.x,
            //                            o.transform.position.y,
            //                            Target.position.z);
            //o.transform.LookAt(targetPostition);
        }
    }
}
