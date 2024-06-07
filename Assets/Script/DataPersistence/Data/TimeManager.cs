using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour, IDataPersistence
{
    public static TimeManager instance;

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
        }
    }

    public void StartGame()
    {
        isGameRunning = true;
    }

    public void StopGame()
    {
        isGameRunning = false;
    }

    public float GetPlayedTime()
    {
        return playedTime;
    }

    public void LoadData(GameData data)
    {
        playedTime = data.playedTime;
    }

    public void SaveData(GameData data)
    {
        data.playedTime = playedTime;
    }
}
