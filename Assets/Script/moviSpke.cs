using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moviSpke : MonoBehaviour
{
    public Transform pos1, pos2;
    public float velocidade;
    public Transform posInicial;
    private Vector3 proximaPos;

    void Update()
    {
        if (transform.position == pos1.position)
        {
            proximaPos = pos2.position;
        }
        if (transform.position == pos2.position)
        {
            proximaPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, proximaPos, velocidade * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
