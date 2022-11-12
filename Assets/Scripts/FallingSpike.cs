using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(BoxCollider2D))
        {
            gameObject.transform.GetComponentInParent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
