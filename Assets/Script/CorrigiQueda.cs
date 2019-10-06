using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrigiQueda : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
