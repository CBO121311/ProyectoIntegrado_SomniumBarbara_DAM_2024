using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public string ID;
    public string type;
    public string description;
    public Sprite icon;

    [HideInInspector]
    public bool pickedUp;

    [HideInInspector]
    public bool equipped;

    [HideInInspector]
    public GameObject weaponManager;

    public bool playerWeapon;

    [HideInInspector]
    public GameObject weapon;

    private void Start()
    {
        weaponManager = GameObject.FindWithTag("WeaponManager");
        if(!playerWeapon )
        {
            int allweapon = weaponManager.transform.childCount;

            for( int i = 0; i < allweapon; i++ )
            {
                //El objeto que tenemos que activar
                if (weaponManager.transform.GetChild(i).gameObject.GetComponent<ItemInventory>().ID == ID)
                {
                    weapon = weaponManager.transform.GetChild(i).gameObject;
                }
            }
        }
    }

    private void Update()
    {
        if (equipped)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                equipped = false;
            }
            if(equipped == false)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void ItemUsage()
    {
        if(type == "Weapon")
        {
            weapon.SetActive(true);
            weapon.GetComponent<ItemInventory>().equipped = true;
        }
    }
}
