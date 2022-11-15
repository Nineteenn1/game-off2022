using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool canPassLevel = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canPassLevel = true;
        FindObjectOfType<AudioManager>().Play("KeyPickup");
        gameObject.SetActive(false);
    }
}
