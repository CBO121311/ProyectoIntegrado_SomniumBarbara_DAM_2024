using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IDataPersistence
{
    public AudioSource clip;

    private string id;
    [SerializeField] private GameObject collectAnimation;
    [SerializeField]private ItemTemplate itemTemplate;
    private SpriteRenderer visual;
    private bool collected = false;
    private Collider2D collider2D;


    //Genera un nuevo identificador único global 
    /*[ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }*/


    private void Awake()
    {
        visual = GetComponentInChildren<SpriteRenderer>();
        visual.sprite = itemTemplate.image;
        id = itemTemplate.id;
        collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectItem();
        }
    }
    private void CollectItem()
    {
        collected = true;
        visual.gameObject.SetActive(false);
        GameEventsManager.instance.ItemCollect();
        collectAnimation.SetActive(true);
        clip.Play();
        Destroy(gameObject, 0.5f);
        if (collider2D != null)
        {
            collider2D.enabled = false;
        }
    }


    // Método para cargar los datos del objeto desde un objeto GameData
    public void LoadData(GameData data)
    {
        // Intenta obtener el estado de recolección del objeto desde los datos proporcionados
        //data.itemsCollected.TryGetValue(id, out collected);

        // Si el objeto ya ha sido recolectado, lo oculta visualmente
        /*if (collected)
        {
            visual.gameObject.SetActive(false);
        }*/
    }

    // Método para guardar los datos del objeto en un objeto GameData
    public void SaveData(GameData data)
    {
        // Si el objeto ya existe en los datos, se elimina para evitar duplicados
        /*if (data.itemsCollected.ContainsKey(id))
        {
            data.itemsCollected.Remove(id);
        }*/
        // Agrega el estado de recolección actual del objeto a los datos
        //data.itemsCollected.Add(id, collected);
    }

    // Método para recolectar el objeto
 
}
