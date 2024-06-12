using UnityEngine;

public class BedroomSceneManager : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset newGameDialogueJSON;
    [SerializeField] private TextAsset loadGameDialogueJSON;
    [SerializeField] private TextAsset wakeUpJSON;
    [SerializeField] private TextAsset nightmareJSON;

    private TextAsset dialogueJSON;

    [SerializeField] private ControllerPlayer_TopDown controllerPlayer;
    private TimeManager timeManager;
    [SerializeField] private Transform player;

    private void Awake()
    {
        timeManager = FindFirstObjectByType<TimeManager>();
    }


    private void Start()
    {

        timeManager.StartGame();
        controllerPlayer.ChangeStatePlayer();

        //Invoke("Init",1f);

        Init();
    }

    private void Init()
    {
        if (PlayerState.nightmare)
        {
            player.position = new Vector2(-1.37f, -12.58f);
            dialogueJSON = nightmareJSON;
            PlayerState.nightmare = false;
        }
        else if (PlayerState.wakeUp)
        {
            player.position = new Vector2(-1.37f, -12.58f);
            dialogueJSON = wakeUpJSON;
            PlayerState.wakeUp = false;
        }
        else
        {
            GameData gameData = DataPersistenceManager.instance.GetGameData();

            if (gameData.isNewGame)
            {
                //player.position = new Vector2(-4.92f, -13.73f);
                dialogueJSON = newGameDialogueJSON;
                gameData.isNewGame = false; // Marcar que ya no es una nueva partida
            }
            else
            {
                dialogueJSON = loadGameDialogueJSON;
                gameData.shouldShowEndOfDayDialogue = false; // Marcar que ya se mostró el diálogo
            }
        }

        Invoke("StartDialogue", 0.5f);

        controllerPlayer.ChangeStatePlayer();
    }

    private void StartDialogue()
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
