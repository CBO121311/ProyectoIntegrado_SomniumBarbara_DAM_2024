using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeLevelText;
    [SerializeField] private TextMeshProUGUI timeScoreText;
    [SerializeField] private TextMeshProUGUI itemLevelText;
    [SerializeField] private TextMeshProUGUI itemScoreText;
    [SerializeField] private TextMeshProUGUI enemiesLevelText;
    [SerializeField] private TextMeshProUGUI enemiesScoreText;
    [SerializeField] private TextMeshProUGUI totalScoreLevelText;



    private float gameTimeInSeconds;
    private int itemsCollected;
    private int enemiesDefeated;

    private const int pointsPerEnemyDefeated = 100;
    private const int pointsPerItemCollected = 1000;
    private const int timeBonusThresholdInSeconds = 120; // 2 minutes
    private const int pointsPerSecondUnderThreshold = 200;

    // Método para establecer los datos del nivel
    public void SetLevelData(float gameTime, int items, int enemies)
    {
        gameTimeInSeconds = gameTime;
        itemsCollected = items;
        enemiesDefeated = enemies;

        /*Debug.Log("gameTimeInSeconds = " + gameTimeInSeconds +
            "itemsCollected = " + itemsCollected +
            "enemiesDefeated = " + enemiesDefeated);*/

        // Mostrar los datos del nivel en los textos correspondientes
        UpdateLevelUI();
        CalculateAndShowScores();
    }

    // Método para calcular y mostrar las puntuaciones
    public void CalculateAndShowScores()
    {
        int timeScore = CalculateTimeScore();
        int itemScore = itemsCollected * pointsPerItemCollected;
        int enemiesScore = enemiesDefeated * pointsPerEnemyDefeated;

        int totalScore = timeScore + itemScore + enemiesScore;

        // Mostrar las puntuaciones en los textos correspondientes
        timeScoreText.text = timeScore.ToString();
        itemScoreText.text = itemScore.ToString();
        enemiesScoreText.text = enemiesScore.ToString();
        totalScoreLevelText.text = totalScore.ToString();
    }

    // Método para calcular la puntuación del tiempo
    private int CalculateTimeScore()
    {
        int timeBonus = 0;
        if (gameTimeInSeconds < timeBonusThresholdInSeconds)
        {
            float timeUnderThreshold = timeBonusThresholdInSeconds - gameTimeInSeconds;
            timeBonus = Mathf.RoundToInt(timeUnderThreshold) * pointsPerSecondUnderThreshold;
        }
        return timeBonus;
    }

    // Método para actualizar los textos que muestran los datos del nivel
    private void UpdateLevelUI()
    {
        timeLevelText.text = FormatTime(gameTimeInSeconds);
        itemLevelText.text = itemsCollected.ToString();
        enemiesLevelText.text = enemiesDefeated.ToString();
    }

    // Método para formatear el tiempo en formato mm:ss:ms
    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60);
        int milliseconds = Mathf.FloorToInt((seconds - Mathf.Floor(seconds)) * 1000);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, remainingSeconds, milliseconds);
    }

}
