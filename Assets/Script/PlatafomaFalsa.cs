using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatafomaFalsa : MonoBehaviour
{

    public GameObject plataforma;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            plataforma.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

}
