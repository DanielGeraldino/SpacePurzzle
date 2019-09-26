using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatafomaFalsa : MonoBehaviour
{

    // TODO: cria uma forma do personagem entra por baixo na plataforma.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

}
