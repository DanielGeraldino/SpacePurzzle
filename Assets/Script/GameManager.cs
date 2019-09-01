using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Painel;
    private bool isPause;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        if (isPause)
        {
            Painel.SetActive(false);
            isPause = false;
        }
        else
        {
            Painel.SetActive(true);
            isPause = true;
        }
    }
}
