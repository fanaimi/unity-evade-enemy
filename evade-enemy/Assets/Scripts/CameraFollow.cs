using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    [SerializeField] private Transform target; 	// the transform to follow
    [SerializeField] Vector3 offset;		// x, y, z, offset, distance from target
    
    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
    
}