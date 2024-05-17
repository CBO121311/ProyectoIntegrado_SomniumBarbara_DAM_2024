using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInfo : MonoBehaviour, IDataPersistence
{
    public void Awake()
    {
        gameObject.SetActive(false);
    }


    [SerializeField] private TextMeshProUGUI titleLevel;
    [SerializeField] private TextMeshProUGUI totalItem;
    [SerializeField] private TextMeshProUGUI timeLevel;
    [SerializeField] private TextMeshProUGUI itemCollected;
    [SerializeField] private TextMeshProUGUI itemMin;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI available;
    private int itemCount = 0;


    public void SetLevelInfo(Level level)
    {
        titleLevel.text = $"Nivel {level.name}";
        totalItem.text = level.totalItems.ToString();
        itemCollected.text = level.collectedItems.ToString();
        timeLevel.text = level.time.ToString() + " segundos";
        itemMin.text = level.minItems.ToString() + " objetos";
        available.text = level.available ? "Pulsa acción para continuar" : "Este nivel no está disponible";
        bestScore.text = level.bestScore.ToString();
    }


    // Método para obtener el número de ítems recolectados
    /*public int GetCollectedItems()
    {
        return currentCollectedItems;
    }*/

    // Método para actualizar el mejor puntaje
    /*public void UpdateBestScore(int newScore)
    {
        currentBestScore = newScore;
        bestScore.text = currentBestScore.ToString();
    }*/

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, bool> pair in data.itemsCollected)
        {
            //Debug.Log(pair.Key + " " + pair.Value);

            if (pair.Value)
            {
                itemCount++;
            }
        }
        itemCollected.text = itemCount.ToString();
    }

    public void SaveData(GameData data)
    {
        // No se utiliza
    }
}
