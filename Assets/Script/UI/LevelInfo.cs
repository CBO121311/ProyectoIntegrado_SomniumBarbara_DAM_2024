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
    [SerializeField] private TextMeshProUGUI itemCollected;
    [SerializeField] private TextMeshProUGUI timeLevel;
    [SerializeField] private TextMeshProUGUI itemMin;
    [SerializeField] private TextMeshProUGUI available;
    private int itemCount = 0;

    // Método para establecer el título del nivel
    public void SetTitleLevel(string title)
    {
        titleLevel.text = $"Nivel {title}";
    }

    // Método para establecer el número total de ítems en el nivel
    public void SetTotalItem(int total)
    {
        totalItem.text = total.ToString();
    }

    // Método para establecer el tiempo restante en el nivel
    public void SetTimeLevel(int time)
    {
        timeLevel.text = time.ToString() + "s";
    }

    // Método para establecer la cantidad mínima de ítems requeridos en el nivel
    public void SetItemMin(int min)
    {
        itemMin.text = min.ToString() + " objetos";
    }

    // Método para establecer si el nivel está disponible o no
    public void SetAvailable(bool isAvailable)
    {
        available.text = isAvailable ? "Pulsa acción para continuar" : "Este nivel no está disponible";
    }

  
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
