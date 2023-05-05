using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrientation : MonoBehaviour
{
    [Header("References")]
    public Transform orientation; 
    public Transform player;  

    private void FixedUpdate()
    {
        Vector3 viewdir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);       
        orientation.forward = viewdir.normalized;       
    }

}
