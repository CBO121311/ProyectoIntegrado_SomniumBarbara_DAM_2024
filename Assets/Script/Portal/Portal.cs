using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour, IDataPersistence
{

    [SerializeField] private Animator portalAnimator;
    [SerializeField] private GameObject panelInfo;
    [SerializeField] private LevelSelection levelSelection;
    [SerializeField] private LevelInfo levelInfo;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private GameObject player;

    [Header("Level Data")]
    public string levelName = "Ardilla";

    private bool playerInRange = false;
    private Level levelToShow;
    //private ControllerPlayerSquirrel playerMovement;

    private void Start()
    {
        //playerMovement = player.GetComponent<ControllerPlayerSquirrel>();
        if (panelInfo != null) panelInfo.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !UIManager_SelectionLevel.GetInstance().levelSelectionIsActive)
        {
            if (InputManager.GetInstance().GetSubmitPressed())
            {
                ActivatePortal();
            }
        }
    }

    private void ActivatePortal()
    {
        UIManager_SelectionLevel.GetInstance().ActivateSelectLevel();

        if (levelToShow != null)
        {
            levelInfo.SetLevelInfo(levelToShow);
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
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            speechBubble.SetActive(true);
            portalAnimator.SetBool("Player", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            speechBubble.SetActive(false);
            playerInRange = false;
            portalAnimator.SetBool("Player", false);     
        }
    }

    public void LoadData(GameData data)
    {
        levelToShow = data.GetLevelByName(levelName,2);
    }

    public void SaveData(GameData data)
    {
        //throw new System.NotImplementedException();
    }
}
