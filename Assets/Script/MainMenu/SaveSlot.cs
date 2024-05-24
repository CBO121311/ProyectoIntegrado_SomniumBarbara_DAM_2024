using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI timeGameText;
    [SerializeField] private TextMeshProUGUI percentageCompleteText;
    [SerializeField] private TextMeshProUGUI deathCountText;


    [Header("Clear Data Button")]
    [SerializeField] private Button clearButton;

    //Establecemos uno en caso que no tengamos datos.
    public bool hasData { get; private set; } = false;
    private Button saveSlotButton;



    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        //No hay datos para este profileId
        if (data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearButton.gameObject.SetActive(false);
        }
        //Hay datos para este profileId
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearButton.gameObject.SetActive(true);


            //Mostramos  los datos en el TextMeshPro
            percentageCompleteText.text = data.GetPercentageComplete() + "% COMPLETADO";
            //Debug.Log(data.GetPercentageComplete());

            deathCountText.text = "Pesadillas: " + data.GetTotalDeaths();
            //Debug.Log(data.playedTime);
            UpdatePlayedTimeText(data.playedTime);
        }
    }

    private void UpdatePlayedTimeText(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timeGameText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }



    //Devolver el id.
    public string GetProfileID()
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearButton.interactable = interactable;
    }
}
