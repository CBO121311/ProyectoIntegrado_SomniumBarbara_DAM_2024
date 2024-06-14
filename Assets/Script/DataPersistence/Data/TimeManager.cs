using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour, IDataPersistence
{
    public static TimeManager instance;
    public static int currentDay { get; private set; }
    private static float playedTime = 0f;
    private static bool isGameRunning = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Hay más de un  TimeManager en la escena. Se destruye el más nuevo");
        }
    }


    private void Update()
    {
        if (isGameRunning)
        {
            playedTime += Time.deltaTime;
            UpdatePlayedTimeText(playedTime);
        }
    }


    private void UpdatePlayedTimeText(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        Debug.Log($"TimeManager : {hours:D2}:{minutes:D2}:{seconds:D2}");
    }

    public void StartGame()
    {
        Debug.Log("Game started");
        if (!isGameRunning)
        {
            isGameRunning = true;
        }
    }

    public void StopGame()
    {
        Debug.Log("Game stopped");
        isGameRunning = false;
    }

    public void Reset()
    {
        playedTime = 0f;
        currentDay = 0;
        isGameRunning = false;
        //Debug.Log("TimeManager reseteado. Tiempo jugado: " + playedTime + ", Día actual: " + currentDay);
    }

    public void plusDays(int days)
    {
        //Debug.Log("Game stopped");
        currentDay += days;
    }

    public float GetPlayedTime()
    {
        return playedTime;
    }

    public void LoadData(GameData data)
    {
        Debug.Log("isGameRunning" + isGameRunning);

        // Si el juego ya está en marcha, no se vuelve a cargar
        if (isGameRunning) return;

        Debug.Log("CARGANDO");
        playedTime = data.playedTime;
        currentDay = data.daysGame;
    }

    public void SaveData(GameData data)
    {
        Debug.Log($"Saving playedTime: {playedTime}");
        data.playedTime = playedTime;
        data.daysGame = currentDay + 1;
    }
}
