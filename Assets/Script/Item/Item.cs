using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IDataPersistence
{
    public AudioSource clip;
    [SerializeField] private string id;

    //Genera un nuevo identificador único global 
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private SpriteRenderer visual;
    [SerializeField] private GameObject collectAnimation;
    private bool collected = false;



    private void Awake()
    {
        visual = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    public void LoadData(GameData data)
    {
        data.itemsCollected.TryGetValue(id, out collected);

        if (collected)
        {
            visual.gameObject.SetActive(false);
        }

    }

    public void SaveData(GameData data)
    {
        if (data.itemsCollected.ContainsKey(id))
        {
            data.itemsCollected.Remove(id);
        }
        data.itemsCollected.Add(id, collected);
    }

    private void CollectCoin()
    {
        collected = true;
        visual.gameObject.SetActive(false);
        GameEventsManager.instance.ItemCollect();
        collectAnimation.SetActive(true);
        clip.Play();
        Destroy(gameObject, 0.5f);
    }
}
