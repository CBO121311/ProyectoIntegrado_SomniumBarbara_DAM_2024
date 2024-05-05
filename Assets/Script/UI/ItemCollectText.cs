using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollectText : MonoBehaviour
{
    private TextMeshProUGUI itemCollectedText;
    [SerializeField] private GameObject itemParents;
    private GameController gameController;

    private void Awake()
    {
        itemCollectedText = this.GetComponent<TextMeshProUGUI>();
        gameController = FindFirstObjectByType<GameController>();
    }

    private void Update()
    {
        itemCollectedText.text = gameController.ItemsCollected + " / " + gameController.ItemCount;
    }
}
