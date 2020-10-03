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

    /// <summary>
    /// Works as a button from editor
    /// </summary>
    public bool Create;
    public int NumberOfColliders;
    public float Radius;
    public Transform PrefabObject;

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
                DoRotate(Objects);
            }

            if(Create)
            {
                Create = false;
                DoCreate();
            }
        }
    }

    public void DoRotate(List<Transform> objects)
    {
        foreach (var o in objects)
        {
            //o.LookAt(Target);

            var dir = Target.position - o.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            o.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //o.Rotate(o.transform.up, 90f);

            //Vector3 targetPostition = new Vector3(o.transform.position.x,
            //                            o.transform.position.y,
            //                            Target.position.z);
            //o.transform.LookAt(targetPostition);
        }
    }

    public void DoCreate()
    {
        var objects = new List<Transform>();
        var angle = 360f / (float)NumberOfColliders;

        for (int i = 0; i < NumberOfColliders; i++)
        {
            var x1 = Radius * Mathf.Cos(angle * i);
            var y1 = Radius * Mathf.Sin(angle * i);

            var x2 = Radius * Mathf.Sin(angle * i);
            var y2 = Radius * -Mathf.Cos(angle * i);

            var pos1 = new Vector3(x1, y1, Target.position.z);
            var pos2 = new Vector3(x2, y2, Target.position.z);

            var tr1 = Instantiate(PrefabObject, pos1, Quaternion.identity);
            var tr2 = Instantiate(PrefabObject, pos2, Quaternion.identity);
            
            objects.Add(tr1);
            objects.Add(tr2);
        }

        DoRotate(objects);
    }
}
