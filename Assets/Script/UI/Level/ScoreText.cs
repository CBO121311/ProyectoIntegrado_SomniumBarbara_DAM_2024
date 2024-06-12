using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private GameController gameController;

    private void Awake()
    {
        scoreText = this.GetComponent<TextMeshProUGUI>();
        gameController = FindFirstObjectByType<GameController>();
    }

    private void Update()
    {
        if (gameController != null)
        {
            // Actualiza el texto del puntaje
            scoreText.text = gameController.GetCurrentScore().ToString("D6");
        }
    }
}
