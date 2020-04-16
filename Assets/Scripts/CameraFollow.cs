using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float Distance;
    [SerializeField] float targetHeight;
    float x = 0;
    float y = 0;
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (!target)
        {
            return;
        }
        y = target.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(x + 20, y, 0);
        transform.rotation = rotation;
        var posiion = target.position - (rotation * Vector3.forward * Distance + new Vector3(0, -targetHeight, 0));
        transform.position = posiion;
    }
}
