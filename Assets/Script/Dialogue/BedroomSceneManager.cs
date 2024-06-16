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
    [SerializeField] private GameObject tutorial;

    private void Awake()
    {
        timeManager = FindFirstObjectByType<TimeManager>();
        //DialogueManager.OnDialogueComplete += HandleDialogueComplete;
    }

    private void OnDestroy()
    {
        //DialogueManager.OnDialogueComplete -= HandleDialogueComplete; // Desuscribirse del evento
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
                //OpenTutorial();
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

    private void HandleDialogueComplete()
    {
        
    }

    private void OpenTutorial()
    {
        tutorial.SetActive(true);
        Time.timeScale = 0;

        LeanTween.scale(tutorial.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.5f)
            .setIgnoreTimeScale(true)
            .setEase(LeanTweenType.easeOutBack);
    }

    public void CloseTutorial()
    {
        LeanTween.scale(tutorial.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 0.5f)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                Time.timeScale = 1;
                tutorial.SetActive(false);
            });
    }

}
