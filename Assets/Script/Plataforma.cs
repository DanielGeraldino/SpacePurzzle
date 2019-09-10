using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma : MonoBehaviour
{
    public Transform posicao1, posicao2;
    public float velociade;
    public Transform posicaoInicial;
    private Vector3 proximaPosicao;

    void Start()
    {
        proximaPosicao = posicaoInicial.position;
    }

    void Update()
    {
        if(transform.position == posicao1.position)
        {
            proximaPosicao = posicao2.position;
        }
        if (transform.position == posicao2.position)
        {
            proximaPosicao = posicao1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, proximaPosicao, velociade * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(posicao1.position, posicao2.position);
    }
}
