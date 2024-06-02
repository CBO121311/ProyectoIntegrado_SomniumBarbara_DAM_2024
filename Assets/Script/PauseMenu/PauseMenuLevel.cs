using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuLevel : Menu
{
    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopMenu confirmationPopMenu;
    [SerializeField] private LevelSquirrelTransition levelSquirrelTransition;
    private bool pauseIsEnable = false;


    public void TooglePause()
    {
        pauseIsEnable = !pauseIsEnable;

        if (pauseIsEnable)
        {
            SetUpOptionMenu();
        }
        else
        {
            OnBackClicked();
        }
    }
    public void SetUpOptionMenu()
    {
        StartCoroutine(ShowOptionPanel());
    }

    public void OnBackClicked()
    {
        StartCoroutine(HideOptionPanel());
    }

    private IEnumerator ShowOptionPanel()
    {
        ActivateMenuButtons();
        levelSquirrelTransition.OpenPauseMenu();
        yield return new WaitForSecondsRealtime(0.5f);
    }

    //Oculta el menú de pauseSL
    private IEnumerator HideOptionPanel()
    {
        DisableMenuButtons();
        levelSquirrelTransition.ClosePauseMenu();
        yield return new WaitForSecondsRealtime(0.5f);
    }

    //Muestra el menú de pause 

    private void ActivateMenuButtons()
    {
        backButton.interactable = true;
        exitButton.interactable = true;
    }

    private void DisableMenuButtons()
    {
        backButton.interactable = false;
        exitButton.interactable = false;
    }

    public void GoLevelSelection()
    {
        DisableMenuButtons();
        confirmationPopMenu.ActivateMenu("¿Estás seguro que quieres salir de la pantalla?\n\n" +
            "Perderás los objetos recogidos.",
            () =>
            {
                Time.timeScale = 1.0f;
                levelSquirrelTransition.FadeOutAndLoadScene("LevelSelection");
            },

            () =>
            {
                ActivateMenuButtons();
                this.SetFirstSelected(exitButton);
            });
    }
}
