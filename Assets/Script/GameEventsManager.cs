    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventsManager : MonoBehaviour
{
    public event Action onPlayerDeath;
    public event Action onItemCollected;
    public event Action onDeadEnemy;
    public event Action onLevelCompleted;
    public event Action<float> onHitEnemy;
    public event Action onFallPlayer;

    public static GameEventsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontró más de un GamesEventsManager.");
        }
        instance = this;
    }



    public void HitEnemy(float damage)
    {
        if (onHitEnemy != null)
        {
            onHitEnemy(damage);
        }
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

    public void DeadEnemy()
    {
        if (onDeadEnemy != null)
        {
            onDeadEnemy();
        }
    }
    public void LevelCompleted()
    {
        if (onLevelCompleted != null)
        {
            onLevelCompleted();
        }
    }



    public void FallPlayer()
    {
        if (onFallPlayer != null)
        {
            onFallPlayer();
        }
    }

}
