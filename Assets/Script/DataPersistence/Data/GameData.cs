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
        AddItemToCollected("1A_01", false);
        AddItemToCollected("1A_02", false);
        AddItemToCollected("1A_03", false);
        AddItemToCollected("1A_04", false);
        AddItemToCollected("1A_05", false);
        AddItemToCollected("2A_01", false);
        AddItemToCollected("2A_02", false);
        AddItemToCollected("2A_03", false);
        AddItemToCollected("2A_04", false);
    }

    private void InitializeLevel()
    {
        informationLevel.Add(new Level("Ardilla", 1, 4, 20, 2, true));
        informationLevel.Add(new Level("Ardilla", 2, 5, 30, 3, true));
        informationLevel.Add(new Level("Mariposa", 1, 0, 0, 0, false));
        informationLevel.Add(new Level("Mariposa", 2, 0, 0, 0, false));
        informationLevel.Add(new Level("Pez", 1, 0, 0, 0, false));
        informationLevel.Add(new Level("Pez", 2, 0, 0, 0, false));
    }


    private void AddItemToCollected(string itemId, bool collected)
    {
        if (!itemsCollected.ContainsKey(itemId))
        {
            itemsCollected.Add(itemId, collected);
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
    public Level GetLevelByName(string nameLevel, int numLevel)
    {
        return informationLevel.Find(level => level.name == nameLevel);
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

    public void LoadInventory(Inventory inventory)
    {
        foreach (var item in itemsCollected)
        {
            if (item.Value)
            {
                inventory.AddItemById(item.Key);
            }
        }
    }

    public List<String> GetListItemsCollected()
    {
        List<String> list = new List<string>();

        foreach(var item in itemsCollected)
        {
            if (item.Value)
            {
                list.Add(item.Key);
            }
        }
        return list;
    }
}
