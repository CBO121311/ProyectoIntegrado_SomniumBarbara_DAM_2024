using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventsManager : MonoBehaviour,IDataPersistence
{
    public GameObject transition;
    public GameObject collection;

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

        if(collection != null)
        {
            AllItemCollected();
        }

    }

    public void AllItemCollected()
    {
        if (collection.transform.childCount == 0)
        {
            Debug.Log("No quedan artículos");
            transition.SetActive(true);
            Invoke("ChangeScene", 1);
        }

        Debug.Log("Quedan = " + collection.transform.childCount + " items");
    }

    void ChangeScene()
    {
        Debug.Log("Enhorabuena, te lo has pasado");
        //SceneManager.LoadScene("SelectionLevel");
    }

    public void LoadData(GameData data)
    {
        Debug.Log("UTILIZANDO LOAD DATA EN GAME EVENTS MANAGER");
        //throw new NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        Debug.Log("UTILIZANDO SAVE DATA EN GAME EVENTS MANAGER");

        /*if (data.itemsCollected.ContainsKey(id))
        {
            data.itemsCollected.Remove(id);
        }*/
    }
}
