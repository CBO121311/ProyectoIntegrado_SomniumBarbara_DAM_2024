using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassArea : MonoBehaviour
{
    [SerializeField] private ScoreLevel scoreLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.LevelCompleted();
        }
    }
}
