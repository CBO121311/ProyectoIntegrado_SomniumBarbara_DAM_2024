using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Para la última partida de guardado.
    public long lastUpdated;
    public int deathCount;
    public SerializableDictionary<string, bool> itemsCollected;
    public Vector3 playerPosition;
    //public ItemsData playerAttributesData;
    public float playedTime;

    public List<Level> informationLevel;


    //los valores definidos en este constructor serán los valores predeterminados 
    //el juego comienza cuando no hay datos para cargar
    public GameData()
    {
        this.deathCount = 0;
        playerPosition = new Vector3(0.58f, -9.63f, 0f);
        playedTime = 0f;
        itemsCollected = new SerializableDictionary<string, bool>();
        informationLevel = new List<Level>();

        InitializeLevel();
        InitializeItemsCollected();
    }

    private void InitializeItemsCollected()
    {
        AddItemToCollected("01A01", false);
        AddItemToCollected("01A02", false);
        AddItemToCollected("01A03", false);
        AddItemToCollected("01A04", false);
        AddItemToCollected("01A05", false);
        AddItemToCollected("01A06", false);
        AddItemToCollected("01A07", false);
        AddItemToCollected("01A08", false);
        AddItemToCollected("01A09", false);
        AddItemToCollected("01B01", false);
    }

    private void InitializeLevel()
    {
        informationLevel.Add(new Level("Ardilla",9,120,5, true));
        informationLevel.Add(new Level("Mariposa", 12, 70, 7, false));
        informationLevel.Add(new Level("Pez", 7, 120, 3, false));
    }


    private void AddItemToCollected(string itemId, bool initialValue)
    {
        if (!itemsCollected.ContainsKey(itemId))
        {
            itemsCollected.Add(itemId, initialValue);
        }
    }

    //Obtiene la cantidad de items recolectados.
    public int GetItemCountCollected()
    {
        int count = 0;
        foreach (bool collected in itemsCollected.Values)
        {
            if (collected)
            {
                count++;
            }
        }
        return count;
    }


    public int GetPercentageComplete()
    {
        //calcula cuantos objetos hemos recolectado
        int totalCollected = 0;
        foreach(bool collected in itemsCollected.Values)
        {
            if(collected)
            {
                totalCollected++;
            }
        }

        //asegurarnos de no dividir por 0 al calcular el porcentaje
        int percentageCompleted = -1;
        if(itemsCollected.Count != 0)
        {
            percentageCompleted = (totalCollected * 100/ itemsCollected.Count);

        }

        //Debug.Log("Porcentaje: " + itemsCollected.Count);

        return percentageCompleted;
    }

    //Método que te devuelve el Level en base al nombre
    public Level GetLevelByName(string levelName)
    {
        return informationLevel.Find(level => level.name == levelName);
    }

    public int GetLevelItemsCollectedPercentage(string levelCode)
    {
        if (string.IsNullOrEmpty(levelCode))
        {
            return 0;
        }

        var filteredItems = itemsCollected.Where(item => item.Key.StartsWith(levelCode)).ToList();

        if (filteredItems.Count == 0)
        {
            return 0;
        }

        int collectedCount = filteredItems.Count(item => item.Value);

        int percentageCollected = (collectedCount * 100) / filteredItems.Count;

        return percentageCollected;
    }


    // Método para obtener la suma de todas las muertes de los niveles.
    public int GetTotalDeaths()
    {
        int totalDeaths = 0;
        foreach (var level in informationLevel)
        {
            totalDeaths += level.deaths;
        }
        return totalDeaths;
    }
}
