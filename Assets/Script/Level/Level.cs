using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public string name;
    public int numlevel;
    public int totalItems;
    public int time;
    public int minItems;
    public int collectedItems;
    public int bestScore;
    public int deaths;
    public bool available;

    public Level(string name, int numlevel,int totalItems, int time, int minItems, bool available)
    {
        this.name = name;
        this.numlevel = numlevel;
        this.totalItems = totalItems;
        this.time = time;
        this.minItems = minItems;
        this.collectedItems = 0;
        this.bestScore = 0;
        this.deaths = 0;
        this.available = available;
    }
}
