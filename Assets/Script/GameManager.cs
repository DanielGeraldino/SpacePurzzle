using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject Painel;
    public bool isPause;
    public float time;
    public Text textTime;

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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void ProximaFase()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SairGame()
    {
        Application.Quit();
    }
}
