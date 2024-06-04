using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelInfo : MonoBehaviour, IDataPersistence
{

    [SerializeField] private TextMeshProUGUI titleLevel;
    [SerializeField] private TextMeshProUGUI totalItem;
    [SerializeField] private TextMeshProUGUI timeLevel;
    [SerializeField] private TextMeshProUGUI itemCollected;
    [SerializeField] private TextMeshProUGUI itemMin;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private GameObject available;

    private int itemCount = 0;


    public void SetLevelInfo(Level level)
    {
        if (level.available)
        {
            available.SetActive(false);
            titleLevel.text = level.name;
            totalItem.text = level.totalItems.ToString();
            timeLevel.text = level.time.ToString() + " segundos";
            itemMin.text = level.minItems.ToString() + " objetos";
            bestScore.text = level.bestScore.ToString();
        }
        else
        {
            available.SetActive(true);
            titleLevel.text = level.name;
            totalItem.text = "";
            timeLevel.text = "";
            itemMin.text = "";
            bestScore.text = "";
            itemCollected.text = "";
        }
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
