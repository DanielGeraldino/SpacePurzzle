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
    // dicionario que guarda um texto/comando relacionado a uma ação/metodo
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>(); 

    private Rigidbody2D rb; // objeto que guarda o rigibody do personagem
    Animator animator; // objeto que guarda o componente animator

    void Start()
    {   
        // iniciando o objeto rb com componete Rigidbody
        rb = GetComponent<Rigidbody2D>();
        // iniciando o objeto rb com componete Animator
        animator = GetComponent<Animator>(); 
        // setando a gravidade inicial na variavel
        gravidadeInicial = rb.gravityScale; 
        personagemVivo = true;
        

        keywords.Add("anda", () => {
            // Adicionando o comando de voz "Anda", se o comando for usado a variavel "AndaParaFrente" fica verdadeira
            andarParaFentre = true;
            andarParaTras = false;
            }); 
        keywords.Add("volta", () => {
            // Adicionando o comando de voz "Volta", se o comando for usado a variavel "AndaParaFente" fica false e "AndarParaTras" fica verdadeira
            andarParaTras = true;
            andarParaFentre = false; 
            }); 
        keywords.Add("sobe", () => { subirEscada = true; }); // Adicionado o comando para subir escada, se o comando for usado a variavel "subirEscada" fica verdadeira
        // Adicionado o comando de parada, se o comando for usado o metodo Parar() é executado
        keywords.Add("para", () => Parar()); 
        // Adicionado o comando Pular, se o comando for executado o metodo Pular() é chamado
        keywords.Add("pula", () => Pular()); 


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
                // se anda para frente é verdadeiro, o metodo Andar() é executado com o paramentro 1. Caso contrario, o metodo recebe o como paramentro -1
                Andar(1); 
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
                // se subirEscada e o personagem estive em contato com a escada. O metodo SubirEscada é executado com paramentro 1. Caso contrario, o paramentro é -1.
                SubirEscada(1); 
            } else if(desceEscada && colisaoEscada)
            {
                SubirEscada(-1);
            }
            else
            {
                // se ambos os casos forem falso. O personagem recebe a gravidade inicial.
                rb.gravityScale = gravidadeInicial; 
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
        // metodo que regarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void finalJogo()
    {

    }

    private void Andar(int direcao)
    {
        // enquando o metodo anda para frente for chamado o personagem será deslocado a (direcao * velocida) no eixo x
        rb.velocity = new Vector2(direcao * velocidadeMovimento, rb.velocity.y); 
        // quando o personagem estive andando, sua animação "walk" é executada.
        animator.SetBool("walking", true); 
        // direcao = 1 para direita. direcao = -1 para esqueda
        if (direcao > 0) 
        {   
            // se a direçao do personagem é para direita, o Sprite não rotaciona para esqueda.
            GetComponent<SpriteRenderer>().flipX = false; 
        } else
        {
            // se a direcao do personagem é para esqueda, o Sprite rotaciona 180° para esquerda.
            GetComponent<SpriteRenderer>().flipX = true; 
        }
    }

    private void SubirEscada(int direcao)
    {
        // enquanto subir escada for verdadeiro, o personagem será deslocado a ( direcao * velocidadeMovimento) no eixo vertical
        rb.velocity = new Vector2(rb.velocity.x, direcao * velocidadeMovimento); 
        // quando o personagem estive subindo escada, sua animação "astroLadder" é executada.
        animator.SetBool("subirEscada", true); 
        // enquanto estive subindo a gravidade do personagem é nula.
        rb.gravityScale = 0; 
    }

    private void Parar()
    {
        if (colisaPlataforma)
        {
            // se o pernagem estive na plataforma, a sua velocidade horizontal é nula
            rb.velocity = new Vector2(0, rb.velocity.y); 
            // a animação de "walk" é parada a execução
            animator.SetBool("walking", false); 
        }
        if (colisaoEscada)
        {
            // se o personagem estive na escada, tando a velocidade horizontal e vertical são anuladas
            rb.velocity = new Vector2(0, 0); 
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
            // metodo AddForce adiciona forca em um dos eixos, neste caso somento no eixo y. Deslocanto o personagem verticalmente
            rb.AddForce(new Vector2(0, forcaPulo)); 
        }
        if (!(colisaPlataforma))
        {
            // ao pular a animação de jump é setada como verdadeira
            animator.SetBool("jumping", true); 
        }
        else
        {
            // caso contrario, ou seja, personagem na platarfoma; A animação de setada como falsa
            animator.SetBool("jumping", false);
        }
    }
    // metodo para indetifica a colisao do personagem com outros objetos do jogo.
    private void OnCollisionEnter2D(Collision2D collision) 
    {   
        // se a tag do objeto em colisão for "Tile", a variavel colisaPlataforma é verdaira
        if (collision.gameObject.CompareTag("Tile") ||
            collision.gameObject.CompareTag("movimentoPlataforma")) 
        {
            colisaPlataforma = true;
        }

        if(collision.gameObject.CompareTag("movimentoPlataforma")){
            GetComponent<Transform>().parent = collision.transform;
        }
        if (collision.gameObject.CompareTag("spike"))
        {
            // se o personagem colidir com os espinhos ele morre
            personagemVivo = false;
            animator.SetBool("morre", true);
        }
        if (collision.gameObject.CompareTag("cristal"))
        {
            qtdCristal++;
        }
        if (collision.gameObject.CompareTag("movimentoPlataforma"))
        {
            // se o personagem colidir com uma plataforma em movimento seu Transform é setado como parente do objeto colidido
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
            // quando sair da colisão com a plataforma em movimento, o personagem deixa de ser filho do objeto em colisão(ou fica sem pai)
            GetComponent<Transform>().parent = null;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // se o personagem entrar em contato com escada, um gatilho é disparado e a variavel colisaoEscada é setada como verdadeira
        if (collision.gameObject.CompareTag("escada")) {
            colisaoEscada = true;
        }
        if (collision.gameObject.CompareTag("portal"))
        {
            // se o personagem colidir com portalOn, a fase é parada e o painelFinal é ativado
            Parar();
            painelFinal.SetActive(true);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // se o personagem sair do contato com escada, um gatilho é disparado e a variavel colisaoEscada é setada como falsa
        if (collision.gameObject.CompareTag("escada"))
        {
            colisaoEscada = false;
            subirEscada = false;
            // seta a animação de subir escada como false
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
