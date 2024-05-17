using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public string name;
    public int totalItems;
    public int time;
    public int minItems;
    public int collectedItems;
    public int bestScore;
    public int deaths;
    public bool available;

    public Level(string name, int totalItems, int time, int minItems, bool available)
    {
        this.name = name;
        this.totalItems = totalItems;
        this.time = time;
        this.minItems = minItems;
        this.collectedItems = 0;
        this.bestScore = 0;
        this.deaths = 0;
        this.available = available;
    }

    // Método para actualizar las propiedades del nivel con los datos de GameData
    public void UpdateLevelData(int collectedItems, int bestScore, int deaths)
    {
        this.collectedItems = collectedItems;
        this.bestScore = bestScore;
        this.deaths = deaths;
    }
}
