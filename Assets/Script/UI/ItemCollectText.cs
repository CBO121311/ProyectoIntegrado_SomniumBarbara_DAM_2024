using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollectText : MonoBehaviour, IDataPersistence
{
    [SerializeField] private int totalItem = 0;

    private int itemsCollected = 0;

    private TextMeshProUGUI itemCollectedText;

    private void Awake()
    {
        itemCollectedText = this.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        // subscribe to events
        GameEventsManager.instance.onItemCollected += OnItemCollected;
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, bool> pair in data.itemsCollected)
        {
            if (pair.Value)
            {
                itemsCollected++;
            }
        }
    }

    public void SaveData(GameData data)
    {
        // no data needs to be saved for this script
    }

    private void OnDestroy()
    {
        // unsubscribe from events
        GameEventsManager.instance.onItemCollected -= OnItemCollected;
    }

    private void OnItemCollected()
    {
        itemsCollected++;
    }

    private void Update()
    {
        itemCollectedText.text = itemsCollected + " / " + totalItem;
    }
}
