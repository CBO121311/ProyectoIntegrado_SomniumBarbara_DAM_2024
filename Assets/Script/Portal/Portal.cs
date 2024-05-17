using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IDataPersistence
{

    [SerializeField] private Animator portalAnimator;
    [SerializeField] private GameObject panelInfo;
    [SerializeField] private LevelInfo levelInfo;
    private bool playerInRange;

    //public GameData gameData;


    [Header("Level Data")]
    public string levelName = "Ardilla";

    private Level levelToShow; 

    private void Awake()
    {
        playerInRange = false;
    }

    private void Update()
    {
        //No podremos activar el dialogo hasta que finalice
        if (playerInRange)
        {
            portalAnimator.SetBool("Player", true);
            panelInfo.SetActive(true);
            panelInfo.transform.position = new Vector2(400, 300);
            //FillLevelInfo();

            
            if (levelToShow != null)
            {
                Debug.Log("Level no es null");
                levelInfo.SetLevelInfo(levelToShow);
            }


            if (InputManager.GetInstance().GetSubmitPressed() && !UIManager.GameIsPaused)
            {
                if (levelToShow != null && levelToShow.available)
                {
                    // Cargar la escena del nivel si está disponible
                    SceneManager.LoadScene("SquirrelLevel");
                }                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            portalAnimator.SetBool("Player", false);
            if (panelInfo != null)
            {
                panelInfo.SetActive(false);
            }
        }
    }

    public void LoadData(GameData data)
    {
        levelToShow = data.GetLevelByName(levelName);
    }

    public void SaveData(GameData data)
    {
        //throw new System.NotImplementedException();
    }
}
