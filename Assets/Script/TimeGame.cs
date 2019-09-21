using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeGame : MonoBehaviour
{
    public float time;
    public GameManager gameManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.GetComponent<GameManager>().isPause)
        {
            ContaTempo();
            GetComponent<Text>().text = time.ToString("F0");
        }
    }

   public void ContaTempo()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            gameManager.GetComponent<GameManager>().RestartGame();
        }
    }
}
