using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Para la última partida de guardado.
    public long lastUpdated;
    public int deathCount;
    public Vector3 playerPosition;
    //public EjemploAttributesData playerAttributesData;
    public float playedTime;

    //los valores definidos en este constructor serán los valores predeterminados 
    //el juego comienza cuando no hay datos para cargar
    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        playedTime = 0f;
        //coinsCollected = new SerializableDictionary<string,bool>();

        //playerAttributesData = new EjemploAttributesData();
    }
    public int GetPercentageComplete()
    {
        //calcula cuantas monedas hemos recolectado
        /*int totalCollected = 0;
        foreach(bool collected in coinsCollected.Values)
        {
            if(collected)
            {
                totalCollected++;
            }
        }*/

        //asegurarnos de no dividir por 0 al calcular el porcentaje
        int percentageCompleted = -1;
        /*if(coinsCollected.Count != 0)
        {
            percentageCompleted = (totalCollected * 100/ coinsCollected.Count);

        }*/



        //Test
        percentageCompleted = 0;
        return percentageCompleted;
    }
}
