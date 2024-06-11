using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset newGameDialogueJSON;
    [SerializeField] private TextAsset loadGameDialogueJSON;
    [SerializeField] private ControllerPlayer_TopDown controllerPlayer;
    private TimeManager timeManager;

    private void Awake()
    {
        timeManager = FindFirstObjectByType<TimeManager>();

    }

    private void Start()
    {
        timeManager.StartGame();
        controllerPlayer.ChangeStatePlayer();

        Invoke("Init",1f);

        /*GameData gameData = DataPersistenceManager.instance.GetGameData();

        if (gameData.isNewGame)
        {
            
            StartDialogue(newGameDialogueJSON);
            gameData.isNewGame = false; // Marcar que ya no es una nueva partida
        }
        else if (gameData.shouldShowEndOfDayDialogue)
        {
            // Iniciar el diálogo de fin de jornada
            StartDialogue(loadGameDialogueJSON);
            gameData.shouldShowEndOfDayDialogue = false; // Marcar que ya se mostró el diálogo
        }*/
    }

    private void Init()
    {
        GameData gameData = DataPersistenceManager.instance.GetGameData();

        if (gameData.isNewGame)
        {

            StartDialogue(newGameDialogueJSON);
            gameData.isNewGame = false; // Marcar que ya no es una nueva partida
        }
        else
        {
            // Iniciar el diálogo de fin de jornada
            StartDialogue(loadGameDialogueJSON);
            gameData.shouldShowEndOfDayDialogue = false; // Marcar que ya se mostró el diálogo
        }
        controllerPlayer.ChangeStatePlayer();
    }

    private void StartDialogue(TextAsset dialogueJSON)
    {
        var dialogueManager = DialogueManager.GetInstance();
        if (dialogueManager != null)
        {
            dialogueManager.EnterDialogueMode(dialogueJSON);
        }
        else
        {
            Debug.LogError("DialogueManager no está disponible.");
        }
    }

}
