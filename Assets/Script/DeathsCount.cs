using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathsCount : MonoBehaviour, IDataPersistence
{
    private int deathCount = 0;
    private TextMeshProUGUI deathCountText;

    private void Awake()
    {
        deathCountText = this.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        //Susbribirme al evento
        //GameEventsManager.instance.onPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        //Desuscribirme al evento
        //GameEventsManager.instance.onPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {

    }

    private void Update()
    {
        deathCountText.text = "" + deathCount;
    }

    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
    }

    public void SaveData(GameData data)
    {
        data.deathCount = this.deathCount;
    }
}
