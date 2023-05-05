using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerController playerController;

    [SerializeField] private Rigidbody objectRigidbody;

    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
         
    }




}
