using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : Menu
{
  
    [Header("Menu Buttons")]
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button cancelButton;
    [SerializeField] private LevelInfo levelInfo;

    [SerializeField] private Transition_SelectionLevel slTransition;

    private Level levelOne;
    private Level levelTwo;

    private void Awake()
    {
        AddButtonSelectionHandler(level1Button);
        AddButtonSelectionHandler(level2Button);

        // Subscribe to the OnButtonSelected event
        level1Button.GetComponent<ButtonSelectionHandler>().OnButtonSelected += HandleButtonSelected;
        level2Button.GetComponent<ButtonSelectionHandler>().OnButtonSelected += HandleButtonSelected;
    }

    private void AddButtonSelectionHandler(Button button)
    {
        if (button.GetComponent<ButtonSelectionHandler>() == null)
        {
            button.gameObject.AddComponent<ButtonSelectionHandler>();
        }
    }

    private void HandleButtonSelected(Button selectedButton)
    {
        if (selectedButton == level1Button)
        {
            ShowLevelInfo(levelOne);
        }
        else if (selectedButton == level2Button)
        {
            ShowLevelInfo(levelTwo);
        }
    }


    public void ShowLevelOptions(Level level1, Level level2)
    {
        levelOne = level1;
        levelTwo = level2;
        OpenLevelSelection();

        //Poner el primer nivel de forma predeterminada
        ShowLevelInfo(level1);
    }

    public void OpenLevelSelection()
    {
        StartCoroutine(ActivateLevelInfoPanel());
    }


    private void ShowLevelInfo(Level level)
    {

        levelInfo.SetLevelInfo(level);
    }

    public void SelectLevelOne()
    {
        if (levelOne.available)
        {
            slTransition.FadeOutAndLoadScene(levelOne.sceneName);
        }
        
    }


    public void SelectLevelTwo()
    {
        if (levelOne.available)
        {
            slTransition.FadeOutAndLoadScene(levelTwo.sceneName);
        }
    }

    public void SelectCancel()
    {
        StartCoroutine(DisableLevelInfoPanel());
    }

    //Activa la interactuación de los botones
    private void ActivateMenuButtons()
    {
        level1Button.interactable = true;
        level2Button.interactable = true;
        cancelButton.interactable = true;
    }

    //Desactiva la interactuación de los botones
    private void DisableMenuButtons()
    {
        level1Button.interactable = false;
        level2Button.interactable = false;
        cancelButton.interactable = false;
    }


    private IEnumerator ActivateLevelInfoPanel()
    {
        ActivateMenuButtons();
        slTransition.ShowLevelInfoPanel();
        yield return new WaitForSeconds(0.6f);

    }

    private IEnumerator DisableLevelInfoPanel()
    {
        DisableMenuButtons();
        slTransition.HideLevelInfoPanel();
        yield return new WaitForSeconds(0.6f);
        UIManager_SelectionLevel.GetInstance().DisableSelectLevel();
    }
}
