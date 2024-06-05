using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelInfo : MonoBehaviour
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
        if (level != null)
        {
            if (level.available)
            {
                available.SetActive(false);
                titleLevel.text = level.name;
                totalItem.text = level.totalItems.ToString();
                timeLevel.text = level.time.ToString() + " segundos";
                itemMin.text = level.minItems.ToString() + " objetos";
                bestScore.text = level.bestScore.ToString();
                itemCollected.text = GetCollectedItemsPercentage(level).ToString() + " %";
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
                itemCollected.text = "";
            }
        }
        else
        {
            Debug.LogWarning("Se intentó establecer la información del nivel con un objeto Level nulo.");
        }
    }

    private float GetCollectedItemsPercentage(Level level)
    {
        GameData gameData = DataPersistenceManager.instance.GetGameData();
        int collectedCount = level.items.Count(item => gameData.itemsCollected.ContainsKey(item) && gameData.itemsCollected[item]);
        return (float)collectedCount / level.items.Count * 100;
    }
}
