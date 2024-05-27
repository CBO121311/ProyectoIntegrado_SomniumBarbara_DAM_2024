using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public ItemTemplate itemTemplate;
    public bool empty = true;

    public Transform slotIconGameObject;
    private Button button; // Reference to the Button component

    private void Awake()
    {
        slotIconGameObject = transform.GetChild(0);
        button = GetComponent<Button>(); // Get the Button component
        UpdateSlotInteractivity();
    }

    public void UpdateSlot()
    {
        if (itemTemplate != null)
        {
            slotIconGameObject.GetComponent<Image>().sprite = itemTemplate.image;
            empty = false;
            Debug.Log("Casillas llenas");
        }
        else
        {
            empty = true;
            Debug.Log("Casillas vacías");
            
        }
        UpdateSlotInteractivity();
    }

    private void UpdateSlotInteractivity()
    {
        if (button != null)
        {
            button.interactable = !empty;
        }
    }

    public string GetItemName()
    {
        return empty ? "" : itemTemplate.itemName;
    }

    public string GetItemDescription()
    {
        return empty ? "" : itemTemplate.description;
    }


}
