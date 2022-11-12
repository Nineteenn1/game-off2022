using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public bool isTouchingAWall;
    public bool isWallSliding;
    // Start is called before the first frame update
    void Awake()
    {
        isTouchingAWall = false;
        isWallSliding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            isTouchingAWall = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            isTouchingAWall = false;
        }
    }
}
