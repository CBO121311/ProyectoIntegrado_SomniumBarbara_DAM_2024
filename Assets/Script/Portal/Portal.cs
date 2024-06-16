using UnityEngine;

public class Portal : MonoBehaviour, IDataPersistence
{
   
    [SerializeField] private Animator portalAnimator;
    [SerializeField] private GameObject panelInfo;
    [SerializeField] private LevelSelection levelSelection;
    [SerializeField] private GameObject player;

    [Header("Level Data")]
    public string levelName = "Ardilla";


    [Header("Player")]
    private bool playerInRange;
    private ControllerPlayer_TopDown controllerPlayer;

    private Level levelToShow_1;
    private Level levelToShow_2;

    //Para colocar al jugador cuando entre en un portal.
    [SerializeField] private GameObject point;
    private Vector2 pointPosition;

    private void Awake()
    {
        playerInRange = false;
        controllerPlayer = FindFirstObjectByType<ControllerPlayer_TopDown>();
    }

    private void Start()
    {
        if (panelInfo != null) panelInfo.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !UIManager_TopDown.GetInstance().levelSelectionIsActive)
        {
            if (InputManager.GetInstance().GetSubmitPressed() && !UIManager_TopDown.GetInstance().gameIsPaused)
            {
                controllerPlayer.DisableSpeechBubble();
                SavePortalPosition();
                ActivatePortal();
            }else
            {
                controllerPlayer.ActivateSpeechBubble();
            }
        }

    }

    private void SavePortalPosition()
    {
        TemporaryData.lastPortalPosition = player.transform.position;
    }


    private void ActivatePortal()
    {
        if (levelToShow_1 != null && levelToShow_2 != null)
        {
            UIManager_TopDown.GetInstance().ActivateSelectLevel();
            levelSelection.ShowLevelOptions(levelToShow_1, levelToShow_2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            portalAnimator.SetBool("Player", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            portalAnimator.SetBool("Player", false);
            controllerPlayer.DisableSpeechBubble();
        }
    }

    public void LoadData(GameData data)
    {
        levelToShow_1 = data.GetLevelByName(levelName,1);
        levelToShow_2 = data.GetLevelByName(levelName, 2);

        //Debug.Log(levelToShow_1.name  + levelToShow_1.numLevel);
        //Debug.Log(levelToShow_2.name);
    }

    public void SaveData(GameData data)
    {
        // Implementación vacía, ya que no se requiere guardar datos aquí.
    }
}

/*if (!UIManager_SelectionLevel.GetInstance().gameIsPaused && !UIManager_SelectionLevel.GetInstance().inventoryIsActivated &&
           InputManager.GetInstance().GetSubmitPressed())
   {
       if (levelToShow != null && levelToShow.available)
       {
           //TemporaryData.PlayerPosition = player.transform.position;
           TemporaryData.UseTemporaryPosition = true;

           //slTransition.FadeOutAndLoadScene("SquirrelLevel");
       }
   }*/