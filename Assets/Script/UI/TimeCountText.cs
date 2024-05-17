using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCountText : MonoBehaviour, IDataPersistence
{
    private float playedTime = 0f;
    private TextMeshProUGUI playedTimeText;
    private bool isGameRunning = false;

    private void Awake()
    {
        playedTimeText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartGame();
    }


    private void Update()
    {
        if (isGameRunning)
        {
            playedTime += Time.deltaTime;
            UpdateUI(playedTime);
        }
    }

    public void StartGame()
    {
        isGameRunning = true;
        UpdateUI(playedTime);
    }

    private void UpdateUI(float timer)
    {
        // Formatear el tiempo jugado en formato de horas:minutos:segundos
        int hours = Mathf.FloorToInt(playedTime / 3600);
        int minutes = Mathf.FloorToInt((playedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(playedTime % 60);

        // Actualizar el texto de la interfaz de usuario con el tiempo jugado
        playedTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        //Debug.Log(playedTimeText.text);
    }

    public void LoadData(GameData data)
    {
        playedTime = data.playedTime;
    }

    public void SaveData(GameData data)
    {
        Debug.Log("Prueba en TimeCountText " + playedTime);

        data.playedTime = playedTime;
    }
}
