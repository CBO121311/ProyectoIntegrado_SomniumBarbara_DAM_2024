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
    [SerializeField] private GameObject triggerEnd;


    private void Awake()
    {
        timeManager = FindFirstObjectByType<TimeManager>();
    }

    private void Start()
    {

        timeManager.StartGame();
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

                dialogueJSON = newGameDialogueJSON;
                gameData.isNewGame = false; // Marcar que ya no es una nueva partida
            }
            else
            {
                dialogueJSON = loadGameDialogueJSON;
                gameData.shouldShowEndOfDayDialogue = false; // Marcar que ya se mostró el diálogo
            }
            AudioManager.Instance.PlaySFX(9);
        }

        CheckIfAllItemsCollected();

        Invoke("StartDialogue", 0.5f);
    }

    private void StartDialogue()
    {
        controllerPlayer.ChangeStatePlayer();
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

    //Comprueba si se ha llegado al 100% de colección de objetos.
    private void CheckIfAllItemsCollected()
    {
        GameData gameData = DataPersistenceManager.instance.GetGameData();

        Debug.Log(gameData.GetPercentageComplete());
        bool allItemsCollected = gameData.GetPercentageComplete() == 100;

        
        triggerEnd.SetActive(allItemsCollected);
    }
}
