using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public GameObject target;

    [SerializeField] [Range(0.01f, 1f)]
    private float smoothSpeed = 0.2f;

    public  Vector3 offset;

    private Vector3 velocity = Vector3.zero;


    private void Awake()
    {
        

        GameObject[] objs = GameObject.FindGameObjectsWithTag("MainCamera");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
