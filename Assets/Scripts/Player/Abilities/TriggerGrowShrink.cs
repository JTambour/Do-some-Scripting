using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGrowShrink : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetHasTriggered(true);
            }
        }
    }
}
