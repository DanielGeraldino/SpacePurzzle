using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour
{
    public float velocidaMovimento = 1.0f;
    private int cont;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (cont <= 500)
        {
            MovePlataforma(1);
        }
        if (cont > 500 && cont <= 1000)
        {
            MovePlataforma(-1);
        }
        if (cont > 1000)
        {
            cont = 0;
        }

        cont++;
    }

    void MovePlataforma(int direcao)
    {
        rb.velocity = new Vector2(direcao * velocidaMovimento, rb.velocity.y);
    }
}
