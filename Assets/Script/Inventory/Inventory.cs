using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour, IDataPersistence
{
    //Esto se utilizará para el inventario, hay que hacer un prefab de item, recordar.

    private bool inventoryEnabled;
    public GameObject inventory;

    private int allSlots;

    private GameObject[] slot;
    public GameObject slotHolder;
    public List<ItemTemplate> itemTemplates;

    public GameObject itemPrefab; // Prefab del item
    [SerializeField] private SLTransition slTranstion;


    private void Awake()
    {
        allSlots = slotHolder.transform.childCount; //Recogemos los hijos
        slot = new GameObject[allSlots];
        //Debug.Log("allslot es" + allSlots);

        for (int i = 0; i < allSlots; i++)
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
            //inventory.SetActive(true);
            slTranstion.OpenInventory();
        }
        else
        {
            //inventory.SetActive(false);
            slTranstion.CloseInventory();
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



    public void AddItem(GameObject itemObject, string itemId, string itemType, string itemDescription, Sprite itemIcon)
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
        }
    }
    public void AddItemById(string itemId)
    {
        ItemTemplate template = itemTemplates.Find(x => x.id == itemId);

        if (template != null)
        {
            GameObject itemObject = Instantiate(itemPrefab);

            ItemInventory itemInventory = itemObject.GetComponent<ItemInventory>();
            itemInventory.ID = itemId;
            itemInventory.type = "Generic";
            itemInventory.description = template.description;
            itemInventory.icon = template.image;
            
            AddItem(itemObject, itemInventory.ID, itemInventory.type, itemInventory.description, itemInventory.icon);
        }
        else
        {
            Debug.LogWarning("ItemTemplate no encontrado para el ID: " + itemId);
        }
    }

    public void LoadData(GameData data)
    {
        data.LoadInventory(this);
        Debug.Log("Intento de cargar");
    }

    public void SaveData(GameData data)
    {
        
    }
}
