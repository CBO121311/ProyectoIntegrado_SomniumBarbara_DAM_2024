using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IPointerClickHandler
{
    public GameObject item;
    public string ID;
    public string type;
    public string description;
    public bool empty;
    public Sprite icon;


    public Transform slotIconGameObject;

    private void Awake()
    {
        slotIconGameObject = transform.GetChild(0);
    }

    private void Start()
    {
        
    }

    public void UpdateSlot()
    {
        slotIconGameObject.GetComponent<Image>().sprite = icon;
    }

    public void UseItem()
    {
        item.GetComponent<ItemInventory>().ItemUsage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UseItem();
        Debug.Log("Se ha hecho click");
    }
}
