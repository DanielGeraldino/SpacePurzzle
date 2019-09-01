using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlataform : MonoBehaviour
{
    public bool moveDireita;

    private Transform tf;
    private float speed = 0.000002f;

    void Start()
    {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = speed + Time.deltaTime;
        if(move < 1000)
        {
            tf.position = new Vector3(move, tf.position.y, tf.position.z);
        }
    }
}
