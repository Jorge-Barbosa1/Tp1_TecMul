using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform playerTransform;
    private Rigidbody playerRb;
    public Vector3 offset;
    public float speed;

    private void Start()
    {
        playerRb = playerTransform.GetComponent<Rigidbody>();
        
    }

    private void LateUpdate()
    {
        Vector3 playerForward = (playerRb.velocity + playerRb.transform.forward).normalized;

        transform.position = Vector3.Lerp(transform.position, playerRb.position + playerRb.transform.TransformVector(offset) + playerForward * (-5f), speed * Time.deltaTime);

        transform.LookAt(playerTransform) ;
    }
}

