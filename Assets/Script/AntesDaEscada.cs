using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntesDaEscada : MonoBehaviour
{
    public GameObject personagem;
    private float forceMagnitudeInicio;

    private void Start()
    {
        forceMagnitudeInicio = GetComponent<AreaEffector2D>().forceMagnitude;
    }
    void Update()
    {
        if (personagem.GetComponent<ControlePorVoz>().parar)
        {
            GetComponent<AreaEffector2D>().forceMagnitude = 0;
        }
        else
        {
            GetComponent<AreaEffector2D>().forceMagnitude = this.forceMagnitudeInicio;
        }
        
    }
}
