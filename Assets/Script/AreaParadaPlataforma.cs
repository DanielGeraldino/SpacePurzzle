using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaParadaPlataforma : MonoBehaviour
{
    public GameObject plataforma;
    public GameObject jogador;
    private int qtdCristal;

    private void Update()
    {
        qtdCristal = jogador.GetComponent<ControlePorVoz>().qtdCristal;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(qtdCristal >= 1 && collision.gameObject.CompareTag("player"))
        {
            plataforma.GetComponent<Plataforma>().enabled = false;
        }
    }
}
