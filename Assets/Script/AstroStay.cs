using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstroStay : MonoBehaviour
{
    public float velocidadeAndar;
    public float forcaPulo;
    public int qtdPulo = 0;

    private float movimentoPlayerHorizontal;
    private float movimentoPlayerVertical;

    private bool personagemNoChao;
    private bool colisaoEscada;
    private float gravidadeInicial;

    private int qtdCristal = 0;
    public Text qtdCritalText;

    //componentes:
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    public Collision2D plataforma;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        gravidadeInicial = rb.gravityScale;
    }

    void Update()
    {
        qtdCritalText.text = qtdCristal.ToString();

        movimentoPlayerHorizontal = Input.GetAxis("Horizontal");
        movimentoPlayerVertical = Input.GetAxis("Vertical");

        Andar();
        Pular();
        SubirEscada();

        
    }

    private void Andar()
    {
        if(movimentoPlayerHorizontal > 0)
        {
            rb.velocity = new Vector2(movimentoPlayerHorizontal * velocidadeAndar, rb.velocity.y);
            sr.flipX = false;
        }
        if(movimentoPlayerHorizontal < 0)
        {
            rb.velocity = new Vector2(movimentoPlayerHorizontal * velocidadeAndar, rb.velocity.y);
            sr.flipX = true;
        }
        if(movimentoPlayerHorizontal != 0)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }
    }

    private void SubirEscada()
    {
        if(movimentoPlayerVertical != 0 && colisaoEscada)
        {
            rb.velocity = new Vector2(rb.velocity.x, movimentoPlayerVertical * velocidadeAndar);
            
        }
       
        if (colisaoEscada && movimentoPlayerVertical != 0 || colisaoEscada && !(personagemNoChao))
        {
            GetComponent<Animator>().SetBool("subirEscada", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("subirEscada", false);
        }

    }

    private void Pular()
    {
        if(Input.GetKey(KeyCode.Space) && personagemNoChao)
        {
            rb.AddForce(new Vector2(0, forcaPulo));
        }
        if (!(personagemNoChao))
        {
            animator.SetBool("jumping", true);
        }
        else
        {
            animator.SetBool("jumping", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Plataforma") || collision.gameObject.CompareTag("Tile") || collision.gameObject.CompareTag("movimentoPlataforma"))
        {
            personagemNoChao = true;
        }
        if (collision.gameObject.CompareTag("cristal"))
        {
            qtdCristal++;
        }
        if (collision.gameObject.CompareTag("movimentoPlataforma"))
        {
            GetComponent<Transform>().parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Plataforma") || collision.gameObject.CompareTag("Tile"))
        {
            personagemNoChao = false;
        }
        if (collision.gameObject.CompareTag("movimentoPlataforma"))
        {
            GetComponent<Transform>().parent = null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("escada"))
        {
            colisaoEscada = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("escada"))
        {
            colisaoEscada = false;
            rb.gravityScale = gravidadeInicial;
        }
    }
}