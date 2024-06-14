using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SaveSlotsMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;


    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopMenu confirmationPopMenu;

    [Header("Transition")]
    [SerializeField] private MainMenuTransition mainMenuTransition;

    private SaveSlot[] saveSlots;
    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {

        DisableMenuButtons();

        //Cargar partida
        if (isLoadingGame)
        {
            //Actualiza el perfil seleccionado que se utilizará para la persistencia de datos
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileID());
            SaveGameAndLoadScene();
        }
        //Si Nuevo juego pero el juego tiene datos.
        else if (saveSlot.hasData)
        {
            confirmationPopMenu.ActivateMenu("Iniciar un Juego Nuevo con esta ranura sobreescribirá los datos guardados actuales"
                + "\n\n¿Estás seguro?",
                () =>
                {


                    DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileID());
                    DataPersistenceManager.instance.NewGame();

                    TimeManager.instance.Reset();
                    DataPersistenceManager.instance.SaveGame();
                    SaveGameAndLoadScene();

                },
                //Función que se ejecuta si seleccionamos "Cancelar"
                () =>
                {
                    // Todo .come back to this.
                    this.ActivateMenu(isLoadingGame);
                }
                );
        }
        else
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileID());
            DataPersistenceManager.instance.NewGame();
            TimeManager.instance.Reset();
            DataPersistenceManager.instance.SaveGame();
            SaveGameAndLoadScene();
        }
    }

    //Método que Guarda el juego en cualquier momento antes de cargar una nueva escena.
    private void SaveGameAndLoadScene()
    {
        mainMenuTransition.FadeOutAndLoadScene("BedroomScene");
    }

    //Método que se llama cuando intentas borrar la partida.
    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        confirmationPopMenu.ActivateMenu("¿Estás seguro que quieres borrar esta partida?",
            //Función que se ejecuta si seleccionamos "Confirmar"
            () =>
            {
                DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileID());
                ActivateMenu(isLoadingGame);
            },

            //Función que se ejecuta si seleccionamos "Cancelar"
            () =>
            {
                ActivateMenu(isLoadingGame);
            }
            );
    }


    //Volver hacia atrás.
    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);

        //Para saber si está cargando o iniciando un juego.
        this.isLoadingGame = isLoadingGame;

        //Carga todos los perfiles de guardado.
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        //Asegurar que el botón backButton esté habilitado cuando se active el menu.
        backButton.interactable = true;

        GameObject firstSelected = backButton.gameObject;

        //Recorra cada ranura de guardado de la UI y establece el contenido de forma apropiada.
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileID(), out profileData);
            saveSlot.SetData(profileData);

            //Para anular el cargar si no hay partidas.
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                //En caso de que no haya guardados en el load
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }

        }

        //Establecer el primer botón seleccionado.
        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    //Desactivar todos los botones
    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveslot in saveSlots)
        {
            saveslot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
