using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Esto se utilizará para el inventario, hay que hacer un prefab de item, recordar.

    private bool inventoryEnabled;
    public GameObject inventory;

    private int allSlots;
    private int enableSlots;

    private GameObject[] slot;
    public GameObject slotHolder;

    private void Start()
    {
        allSlots = slotHolder.transform.childCount; //Recogemos los hijos

        slot = new GameObject[allSlots];

        for (int i = 0;i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<SlotItem>().item == null)
            {
                slot[i].GetComponent<SlotItem>().empty = true;
            }
        }
    }

    private void Update()
    {
        if (InputManager.GetInstance().GetInteractPressed())
        {
            inventoryEnabled = !inventoryEnabled;
        }

        if (inventoryEnabled)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Item")
        {
            GameObject itemPickedUp = other.gameObject;
            ItemInventory item = itemPickedUp.GetComponent<ItemInventory>();

            AddItem(itemPickedUp, item.ID, item.type, item.description, item.icon);
        }
    }



    public void AddItem(GameObject itemObject, int itemId, string itemType, string itemDescription, Sprite itemIcon)
    {
        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<SlotItem>().empty)
            {
                itemObject.GetComponent<ItemInventory>().pickedUp = true;

                slot[i].GetComponent<SlotItem>().item = itemObject;
                slot[i].GetComponent<SlotItem>().ID = itemId;

                slot[i].GetComponent<SlotItem>().type = itemType;
                slot[i].GetComponent<SlotItem>().description = itemDescription;
                slot[i].GetComponent<SlotItem>().icon = itemIcon;

                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);



                slot[i].GetComponent<SlotItem>().UpdateSlot();


                slot[i].GetComponent<SlotItem>().empty = false;
                return;
            }
            //Para evitar que se añada en todos los slot el inventario.
           
        }
    }
}
