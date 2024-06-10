using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GameData
{
    //Para la última partida de guardado.
    public long lastUpdated;
    public int deathCount;
    public int daysGame;
    public SerializableDictionary<string, bool> itemsCollected;
    //public Vector3 playerPosition;
    public float playedTime;

    public List<Level> informationLevel;

    public bool firstTime;



    //los valores definidos en este constructor serán los valores predeterminados 
    //el juego comienza cuando no hay datos para cargar
    public GameData()
    {
        this.deathCount = 0;
        //playerPosition = new Vector3(0.58f, -9.63f, 0f);
        daysGame = 1;
        playedTime = 0f;
        itemsCollected = new SerializableDictionary<string, bool>();
        informationLevel = new List<Level>();
        this.firstTime = true;

        InitializeLevel();
        InitializeItemsCollected();
    }

    private void InitializeItemsCollected()
    {
        AddItemToCollected("0Z_00", true);
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
        var level1A = new Level("Ardilla", 1, "SquirrelLevel1", 5, 120, 3, true);
        level1A.items.AddRange(new List<string> { "1A_01", "1A_02", "1A_03", "1A_04", "1A_05" });
        informationLevel.Add(level1A);

        var level2A = new Level("Ardilla", 2, "SquirrelLevel2", 4, 100, 2, true);
        level2A.items.AddRange(new List<string> { "2A_01", "2A_02", "2A_03", "2A_04" });
        informationLevel.Add(level2A);

        informationLevel.Add(new Level("Mariposa", 1, "", 0, 0, 0, false));
        informationLevel.Add(new Level("Mariposa", 2, "", 0, 0, 0, false));
        informationLevel.Add(new Level("Pez", 1, "", 0, 0, 0, false));
        informationLevel.Add(new Level("Pez", 2, "", 0, 0, 0, false));
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
    public Level GetLevelByName(string nameLevel,int numLevel)
    {
        //Debug.Log("GetLevelByName" + " " + nameLevel + numLevel);

        return informationLevel.Find(level => level.name == nameLevel && level.numLevel == numLevel);
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
