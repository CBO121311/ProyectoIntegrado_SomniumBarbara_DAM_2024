using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour, IDataPersistence
{
    public static TimeManager instance;
    public int currentDay { get; private set; }
    private float playedTime = 0f;
    private bool isGameRunning = false;

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
        //Debug.Log($"TimeManager : {hours:D2}:{minutes:D2}:{seconds:D2}");
    }

    public void StartGame()
    {
        //Debug.Log("Game started");
        isGameRunning = true;
    }

    public void StopGame()
    {
        //Debug.Log("Game stopped");
        isGameRunning = false;
        playedTime = 0f;
    }

    public float GetPlayedTime()
    {
        return playedTime;
    }

    public void LoadData(GameData data)
    {
        //Si el juego ha comenzado, no se vuelve a cargar
        if (isGameRunning) return;

        //Debug.Log($"Loaded playedTime: {data.playedTime}");
        playedTime = data.playedTime;
        currentDay = data.daysGame;
    }

    public void SaveData(GameData data)
    {
        //Debug.Log($"Saving playedTime: {playedTime}");
        data.playedTime = playedTime;

        data.daysGame = currentDay + 1;
    }
}
