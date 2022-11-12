using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<AudioManager>().Play("LostGameSfx");
            FindObjectOfType<Player>().ResetToSpawnPosition();

            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }

        if (collision.gameObject.tag == "Ground")
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}
