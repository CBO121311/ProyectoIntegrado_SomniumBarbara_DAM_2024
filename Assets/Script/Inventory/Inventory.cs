using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Esto se utilizará para el inventario, hay que hacer un prefab de item, recordar.

    [SerializeField] GameObject prefItemInventory;
    [SerializeField] int numMaxItemInventory;
    [SerializeField] ItemTemplate[] listInventory;

    private ItemObject itemObject;

    private void Start()
    {
        for (int i = 0; i<=numMaxItemInventory; i++)
        {
            GameObject inventory = GameObject.Instantiate(prefItemInventory, Vector2.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("Inventory").transform);
            itemObject = inventory.GetComponent<ItemObject>();
            itemObject.CrearObjeto(listInventory[i]);
        }
    }
}
