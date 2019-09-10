using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech; // Api da microsoft para entrada por voz
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlePorVoz : MonoBehaviour
{
    public float velocidadeMovimento = 4.0f; // variavel que guarda a velocidade do movimento do personagem
    public float forcaPulo = 300.0f; // força do pulo do personagem
    private float gravidadeInicial; // guarda a gravidade inicial do personagem
   
    public bool colisaPlataforma; // identifica se o personagem esta na plataforma(ou chao)
    public bool andarParaFentre; // verdadeiro se o personagem recebe o comando de anda para frente
    public bool andarParaTras; // verdadeiro se o personagem recebe o comando de anda para tras
    public bool colisaoEscada; // identifica se o persogem esta colidido com a escada
    public bool subirEscada; // verdadeiro se o personagem estive subido escada
    public bool desceEscada; // verdadeiro se o personagem recebe o comando de descer escada
    public bool personagemVivo; // indicar se o personagem esta vivo;

    public int qtdCristal = 0; // guarda a quandidade de cristal coletada
    public Text textoQtdCristal; // objeto Text que desenha o a quantidade de cristal na tela do jogo;

    public GameObject gameManager;
    public GameObject objPortal;
    public GameObject painelFinal;

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>(); // dicionario que guarda um texto/comando relacionado a uma ação/metodo

    private Rigidbody2D rb; // objeto que guarda o rigibody do personagem
    Animator animator; // objeto que guarda o componente animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // iniciando o objeto rb com componete Rigidbody
        animator = GetComponent<Animator>(); // iniciando o objeto rb com componete Animator
        gravidadeInicial = rb.gravityScale; // setando a gravidade inicial na variavel
        personagemVivo = true;
        

        keywords.Add("anda", () => {
            andarParaFentre = true;
            andarParaTras = false;
            }); // Adicionando o comando de voz "Anda", se o comando for usado a variavel "AndaParaFrente" fica verdadeira
        keywords.Add("volta", () => {
            andarParaTras = true;
            andarParaFentre = false; 
            }); // Adicionando o comando de voz "Volta", se o comando for usado a variavel "AndaParaFente" fica false e "AndarParaTras" fica verdadeira
        keywords.Add("sobe", () => { subirEscada = true; }); // Adicionado o comando para subir escada, se o comando for usado a variavel "subirEscada" fica verdadeira
        keywords.Add("para", () => Parar()); // Adicionado o comando de parada, se o comando for usado o metodo Parar() é executado
        keywords.Add("pula", () => Pular()); // Adicionado o comando Pular, se o comando for executado o metodo Pular() é chamado


        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (personagemVivo && !(gameManager.GetComponent<GameManager>().isPause))
        {
            textoQtdCristal.text = qtdCristal.ToString();
            Portal portal = objPortal.GetComponent<Portal>();

            if (qtdCristal == 3)
            {
                 portal.portalOn = true;
            }

            if (andarParaFentre)
            {
                Andar(1); // se anda para frente é verdadeiro, o metodo Andar() é executado com o paramentro 1. Caso contrario, o metodo recebe o como paramentro -1
            } else if (andarParaTras)
            {
                Andar(-1);
            }
            else
            {
                andarParaFentre = false;
                andarParaTras = false;
            }

            if (subirEscada && colisaoEscada)
            {
                SubirEscada(1); // se subirEscada e o personagem estive em contato com a escada. O metodo SubirEscada é executado com paramentro 1. Caso contrario, o paramentro é -1.
            } else if(desceEscada && colisaoEscada)
            {
                SubirEscada(-1);
            }
            else
            {
                rb.gravityScale = gravidadeInicial; // se ambos os casos forem falso. O personagem recebe a gravidade inicial.
            }
        } else if (gameManager.GetComponent<GameManager>().isPause)
        {
            Parar();
        } else if (!(personagemVivo))
        {
            Invoke("RecarregarScene", 3f);
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

    private void RecarregarScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void finalJogo()
    {

    }

    private void Andar(int direcao)
    {
        rb.velocity = new Vector2(direcao * velocidadeMovimento, rb.velocity.y); // enquando o metodo anda para frente for chamado o personagem será deslocado a (direcao * velocida) no eixo x
        animator.SetBool("walking", true); // quando o personagem estive andando, sua animação "walk" é executada.
        if(direcao > 0) // direcao = 1 para direita. direcao = -1 para esqueda
        {
            GetComponent<SpriteRenderer>().flipX = false; // se a direçao do personagem é para direita, o Sprite não rotaciona para esqueda.
        } else
        {
            GetComponent<SpriteRenderer>().flipX = true; // se a direcao do personagem é para esqueda, o Sprite rotaciona 180° para esquerda.
        }
    }

    private void SubirEscada(int direcao)
    {
        rb.velocity = new Vector2(rb.velocity.x, direcao * velocidadeMovimento); // enquanto subir escada for verdadeiro, o personagem será deslocado a ( direcao * velocidadeMovimento) no eixo vertical
        animator.SetBool("subirEscada", true); // quando o personagem estive subindo escada, sua animação "astroLadder" é executada.
        rb.gravityScale = 0; // enquanto estive subindo a gravidade do personagem é nula.
    }

    private void Parar()
    {
        if (colisaPlataforma)
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // se o pernagem estive na plataforma, a sua velocidade horizontal é nula
            animator.SetBool("walking", false); // a animação de "walk" é parada a execução
        }
        if (colisaoEscada)
        {
            rb.velocity = new Vector2(0, 0); // se o personagem estive na escada, tando a velocidade horizontal e vertical são anuladas
        }
        // as 3 variaveis de movimentos são setadas para falsas
        andarParaFentre = false;
        andarParaTras = false;
        subirEscada = false;
    }

    private void Pular()
    {
        if (colisaPlataforma)
        {
            rb.AddForce(new Vector2(0, forcaPulo)); // metodo AddForce adiciona forca em um dos eixos, neste caso somento no eixo y. Deslocanto o personagem verticalmente
            
        }
        if (!(colisaPlataforma))
        {
            animator.SetBool("jumping", true); // ao pular a animação de jump é setada como verdadeira
        }
        else
        {
            animator.SetBool("jumping", false); // caso contrario, ou seja, personagem na platarfoma; A animação de setada como falsa
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) // metodo para indetifica a colisao do personagem com outros objetos do jogo.
    {
        if (collision.gameObject.CompareTag("Tile") ||
            collision.gameObject.CompareTag("movimentoPlataforma")) // se a tag do objeto em colisão for "Tile", a variavel colisaPlataforma é verdaira
        {
            colisaPlataforma = true;
            collision.gameObject.CompareTag("movimentoPlataforma");
        }

        if(collision.gameObject.CompareTag("movimentoPlataforma")){
            GetComponent<Transform>().parent = collision.transform;
        }
        if (collision.gameObject.CompareTag("spike"))
        {
            personagemVivo = false;
            animator.SetBool("morre", true);
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

    private void OnCollisionExit2D(Collision2D collision) // metodo para indetifica a saida da colisao do personagem com outros objetos do jogo.
    {
        if (collision.gameObject.CompareTag("Tile")) // se a tag do objeto em colisão era "Tile", a variavel colisaPlataforma é falsa
        {
            colisaPlataforma = false;
        }
        if (collision.gameObject.CompareTag("movimentoPlataforma"))
        {
            GetComponent<Transform>().parent = null;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("escada")) // se o personagem entrar em contato com escada, um gatilho é disparado e a variavel colisaoEscada é setada como verdadeira
        {
            colisaoEscada = true;
        }
        if (collision.gameObject.CompareTag("movimentoPlataforma"))
        {
            GetComponent<Transform>().parent = null;
        }
        if (collision.gameObject.CompareTag("portal"))
        {
            Parar();
            painelFinal.SetActive(true);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("escada"))// se o personagem sair do contato com escada, um gatilho é disparado e a variavel colisaoEscada é setada como falsa
        {
            colisaoEscada = false;
            subirEscada = false;
            animator.SetBool("subirEscada", false); // seta a animação de subir escada como false
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
