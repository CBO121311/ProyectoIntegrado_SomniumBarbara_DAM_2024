using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInfo : MonoBehaviour
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

    // Método para establecer el número de ítems recolectados en el nivel
    public void SetItemCollected(int collected)
    {
        itemCollected.text = collected.ToString();
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
}
