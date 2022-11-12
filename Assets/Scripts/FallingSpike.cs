using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
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
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }
    }

    private void Update()
    {
        if (gameObject.transform.position.y <= -20)
        {
            Destroy(gameObject);
        }
    }
}
