using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;

public class ControlePorVoz : MonoBehaviour
{
    public float velocidadeMovimento = 4.0f;
    public float forcaPulo = 300.0f;
    private float gravidadeInicial;
   

    public bool pontoParada;
    public bool colisaPlataforma;
    public bool andarParaFentre;
    public bool andarParaTras;
    public bool colisaoEscada;
    public bool subirEscada;
    public bool desceEscada;

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    private Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravidadeInicial = rb.gravityScale;

        keywords.Add("anda", () => {
            andarParaFentre = true;
            andarParaTras = false;
            });
        keywords.Add("Volta", () => {
            andarParaTras = true;
            andarParaFentre = false; 
            });
        keywords.Add("sobe", () => { subirEscada = true; });
        keywords.Add("para", () => Parar());
        keywords.Add("pula", () => Pular());


        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (andarParaFentre)
        {
            Andar(1);
        } else if (andarParaTras)
        {
            Andar(-1);
        }

        if (subirEscada && colisaoEscada)
        {
            SubirEscada(1);
        } else if(desceEscada && colisaoEscada)
        {
            SubirEscada(-1);
        }
        else
        {
            rb.gravityScale = gravidadeInicial;
        }

    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if(keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    private void Andar(int direcao)
    {
        // rigidbody.velocity = new Vector2(rigidbody.velocity.x * velocidadeMovimento, rigidbody.velocity.y);
        rb.velocity = new Vector2(direcao * velocidadeMovimento, rb.velocity.y);
        animator.SetBool("walking", true);
        if(direcao > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        } else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void SubirEscada(int direcao)
    {
        rb.velocity = new Vector2(rb.velocity.x, direcao * velocidadeMovimento);
        animator.SetBool("subirEscada", true);
        rb.gravityScale = 0;
    }

    private void Parar()
    {
        if (colisaPlataforma)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("walking", false);
        }
        if (colisaoEscada)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        andarParaFentre = false;
        andarParaTras = false;
        subirEscada = false;
    }

    private void Pular()
    {
        if (colisaPlataforma)
        {
            rb.AddForce(new Vector2(0, forcaPulo));
            
        }
        if (!(colisaPlataforma))
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
        if (collision.gameObject.CompareTag("Tile"))
        {
            colisaPlataforma = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            colisaPlataforma = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("escada"))
        {
            colisaoEscada = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("escada"))
        {
            colisaoEscada = false;
            animator.SetBool("subirEscada", false);
        }
    }

    private void OnDestroy()
    {
        if(keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}
