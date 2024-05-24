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
    bool collected = false;
    private Collider2D collider2D;
    private GameController gameController;

    public bool Collected { get => collected;}
    public string Id { get => id;}

    private void Awake()
    {
        visual = this.GetComponentInChildren<SpriteRenderer>();
        visual.sprite = itemTemplate.image;
        this.id = itemTemplate.id;
        collider2D = this.GetComponent<Collider2D>();
        gameController = FindFirstObjectByType<GameController>();
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
        Invoke("DisableCollectAnimation", 0.5f); 
        if (collider2D != null)
        {
            collider2D.enabled = false;
        }
        
        if(gameController != null)
        {
            gameController.UpdateItemCollectedStatus(id, true);
        }
    }

    // Método para desactivar la animación de recolección
    private void DisableCollectAnimation()
    {
        collectAnimation.SetActive(false);
    }

    // Método para cargar los datos del objeto desde un objeto GameData
    public void LoadData(GameData data)
    {
        //Debug.Log("LoadData de Item");
        // Intenta obtener el estado de recolección del objeto desde los datos proporcionados
        //data.itemsCollected.TryGetValue(id, out collected);

        // Si el objeto ya ha sido recolectado, lo oculta visualmente
        /*if (collected)
        {
            visual.gameObject.SetActive(false);
        }*/
    }

    public void SaveData(GameData data)
    {
    }
}
