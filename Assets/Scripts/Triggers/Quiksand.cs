using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiksand : MonoBehaviour
{
    public GameObject quiksand;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            quiksand.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
