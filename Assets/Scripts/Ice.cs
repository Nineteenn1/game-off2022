using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private int state = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        state++;

        if (state == 3)
            gameObject.SetActive(false);
    }
}
