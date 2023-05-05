using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChatBubble : MonoBehaviour
{
    public GameObject chatBubble;
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("Activate");
            chatBubble.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatBubble.gameObject.SetActive(false);
        }
    }
}
