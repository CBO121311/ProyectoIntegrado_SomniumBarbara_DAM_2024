using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventsManager : MonoBehaviour
{
    public GameObject transition;
    public event Action onPlayerDeath;
    public event Action onItemCollected;

    public static GameEventsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontró más de un administrador de eventos en la escena.");
        }
        instance = this;
    }

    public void PlayerDeath()
    {
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }

    public void ItemCollect()
    {
        if (onItemCollected != null)
        {
            onItemCollected();
        }
    }

    private void Update()
    {
        //AllArticleCollected();
    }

    public void AllArticleCollected()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("No quedan artículos");
            transition.SetActive(true);
            Invoke("ChangeScene", 1);
        }
    }

    void ChangeScene()
    {
        Debug.Log("Enhorabuena, te lo has pasado");
        //SceneManager.LoadScene("SelectionLevel");
    }
}
