using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private Image image; 
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description; 

    public void CrearObjeto(ItemTemplate dataItem)
    {
        id.text = dataItem.id;
        image.sprite = dataItem.image;
        itemName.text = dataItem.itemName;
        description.text = dataItem.description;    
    }
}
