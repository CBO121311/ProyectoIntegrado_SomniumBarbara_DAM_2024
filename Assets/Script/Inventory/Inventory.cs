using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IDataPersistence
{
    //Esto se utilizará para el inventario, hay que hacer un prefab de item, recordar.

    private bool inventoryEnabled =false;
    public GameObject inventory;

    private int allSlots;
    private GameObject[] slot;
    public GameObject slotHolder;
    public List<ItemTemplate> itemTemplates;

    private TopDown_Transition transition;

    // Referencias adicionales
    [SerializeField] private EventSystem eventSystem; 
    [SerializeField] private GameObject firstSlot;

    // Referencias a TextMeshPro
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    private void Awake()
    {
        allSlots = slotHolder.transform.childCount; //Recogemos los hijos
        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;
            SlotItem slotItem = slot[i].GetComponent<SlotItem>();
            slotItem.empty = true;
            slotItem.UpdateSlot();
        }
    }

    private void Start()
    {
        transition = FindFirstObjectByType<TopDown_Transition>();
    }

    private void Update()
    {
        // Maneja la selección del slot
        if (inventoryEnabled && eventSystem.currentSelectedGameObject != null)
        {
            SlotItem selectedSlot = eventSystem.currentSelectedGameObject.GetComponent<SlotItem>();

            if(selectedSlot != null)
            {
                itemNameText.text = selectedSlot.GetItemName();
                if (InputManager.GetInstance().GetSubmitPressed())
                {
                    itemDescriptionText.text = selectedSlot.GetItemDescription();
                }
            }   
        }
    }


    public void ToogleInventory()
    {
        inventoryEnabled = !inventoryEnabled;

        if (inventoryEnabled)
        {
            transition.OpenInventory();
            // Selecciona el primer slot
            if (firstSlot != null && eventSystem != null)
            {
                eventSystem.SetSelectedGameObject(firstSlot);
            }
        }
        else
        {
            transition.CloseInventory();
            // Desactiva la selección cuando se cierra el inventario
            if (eventSystem != null)
            {
                eventSystem.SetSelectedGameObject(null);
            }
        }
    }


    //Actualmente usado para temas de test en LevelSelection
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Item")
        {
            ItemTemplate itemTemplate = other.GetComponent<ItemTemplateHolder>().itemTemplate;
            AddItem(itemTemplate);
        }
    }*/



    //Busca un hueco vacío y añade el item.
    public void AddItem(ItemTemplate itemTemplate)
    {
        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<SlotItem>().empty)
            {
                SlotItem slotItem = slot[i].GetComponent<SlotItem>();
                slotItem.itemTemplate = itemTemplate;
                slotItem.empty = false;


                slotItem.UpdateSlot();
                return;
            }
        }
    }

    //Busca el id del item y si lo encuentra lo añade al inventario
    public void AddItemById(string itemId)
    {
        ItemTemplate template = itemTemplates.Find(x => x.id == itemId);

        if (template != null)
        {
            AddItem(template);
        }
        else
        {
            Debug.LogWarning("ItemTemplate no encontrado para el ID: " + itemId);
        }
    }

    public void LoadData(GameData data)
    {
        data.LoadInventory(this);
        //Debug.Log("Intento de cargar");
    }

    public void SaveData(GameData data)
    {
        
    }
}
