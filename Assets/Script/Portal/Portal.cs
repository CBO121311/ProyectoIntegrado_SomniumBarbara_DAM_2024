using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    [SerializeField] private Animator portalAnimator;
    [SerializeField] private GameObject panelInfo;
    [SerializeField] private LevelInfo levelInfo;
    private bool playerInRange;

    [Header("Level Data")]
    public string levelName = "Example";
    public int totalItems = 3;
    public int timeLevel = 12;
    public int minItems = 3;
    public bool available = false;

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
            FillLevelInfo();

            if (InputManager.GetInstance().GetSubmitPressed() && !UIManager.GameIsPaused)
            {
                if (available)
                {
                    //Debug.Log("Entrando en escena");
                    SceneManager.LoadScene("SquirrelLevel");
                }
                else
                {
                    //Debug.Log("No puedes entrar");
                }

                // Lugar para iniciar un diálogo o cargar una nueva escena
                // DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                // SceneManager.LoadScene(2);
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

    // Método para llenar los campos de texto de LevelInfo con los datos establecidos en el editor
    private void FillLevelInfo()
    {
        levelInfo.SetTitleLevel(levelName);
        levelInfo.SetTotalItem(totalItems);
        levelInfo.SetTimeLevel(timeLevel);
        levelInfo.SetItemMin(minItems);
        levelInfo.SetAvailable(available);
    }
}
