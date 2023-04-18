using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chatbubble : MonoBehaviour
{
    public GameObject chatBubble;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {           
            chatBubble.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        chatBubble.SetActive(false);
    }
}


