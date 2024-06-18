using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ItemCustom", menuName = "Collection Item")]
public class ItemTemplate : ScriptableObject
{
    public string id; // Identificador único del objeto
    public Sprite image; // Imagen asociada al objeto
    public string itemName; // Nombre del objeto
    public string description; // Descripción del objeto
}

public class ItemTemplateHolder : MonoBehaviour
{
    public ItemTemplate itemTemplate;
}