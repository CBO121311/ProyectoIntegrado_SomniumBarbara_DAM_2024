using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : Menu
{
  
    [Header("Menu Buttons")]
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button cancelButton;

    [SerializeField] private Transition_SelectionLevel slTransition;

    public void OpenLevelSelection()
    {
        StartCoroutine(ActivateLevelInfoPanel());
    }

    public void SelectLevelOne()
    {
        slTransition.FadeOutAndLoadScene("SquirrelLevel");
    }


    public void SelectLevelTwo()
    {
        slTransition.FadeOutAndLoadScene("SquirrelLevel2");
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
