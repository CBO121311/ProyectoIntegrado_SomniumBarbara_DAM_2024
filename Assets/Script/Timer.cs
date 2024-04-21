using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timer = 20f;
    public TextMeshProUGUI textoTimer;
    private float lastNumber;

    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        textoTimer.text = timer.ToString("f0");

        if (Time.time - lastNumber >= 1f)
        {
            Debug.Log("Tiempo restante: " + timer.ToString("f0"));
            lastNumber = Time.time;
        }

        if (timer < 0)
        {
            Debug.Log("Tiempo restante: " + timer.ToString("f0"));
            Debug.Log("¡Tiempo agotado! Reiniciando partida...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
        }
    }
}
