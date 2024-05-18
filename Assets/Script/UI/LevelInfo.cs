using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

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
        
        timeLevel.text = level.time.ToString() + " segundos";
        itemMin.text = level.minItems.ToString() + " objetos";
        available.text = level.available ? "Pulsa acción para continuar" : "Este nivel no está disponible";
        bestScore.text = level.bestScore.ToString();
    }


    public void LoadData(GameData data)
    {
        itemCollected.text = data.GetLevelItemsCollectedPercentage("01A").ToString() + " %";
    }

    public void SaveData(GameData data)
    {
        // No se utiliza
    }
}
