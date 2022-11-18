using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpPadPower = 27.0f;

    public Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<Player>().isJumping = true;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPadPower, ForceMode2D.Impulse);
            FindObjectOfType<AudioManager>().Play("PlayerJump");
        }
    }
}
