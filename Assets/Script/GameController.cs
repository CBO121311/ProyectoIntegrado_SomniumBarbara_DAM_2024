using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IDataPersistence
{

    [Header("Timer")]
    [SerializeField] private Slider sliderTime;
    private float maxTime;
    private float currentTime;
    private bool endTime = false;

    [Header("End Timer")]
    [SerializeField] private GameObject reaper;
    [SerializeField] private Animator bgFront;
    [SerializeField] private Animator bgBack;
    private bool deathPlayer = false;


    [Header("Transition Animation")]
    [SerializeField] private LevelSquirrelTransition levelSquirrelTransition;

    [Header("Level Complete")]
    private float levelTime = 0f;
    private bool isGameRunning = true;


    [SerializeField] private GameObject levelCompleteTransition;
    [SerializeField] private ScoreLevel scoreLevel;
    [SerializeField] private PassLevel passLevel;
    public GameObject collection;
    

    private Dictionary<string, bool> itemsLevel;

    //Número total de items
    private int itemCount;
    //Items recogidos
    private int itemsCollected = 0;
    //Enemigos derrotados
    private int deathEnemyCount = 0;
    public int ItemCount { get => itemCount;}
    public int ItemsCollected { get => itemsCollected;}

    [Header("Information Level")]
    [SerializeField] private string nameCurrentLevel;
    [SerializeField] private int numCurrentLevel;
    private Level currentLevel;
    private int numItemMinComplete;

    [SerializeField] private float timeActivateReaper = 5f;


    private bool objectiveCompleted = false;

    public static GameController instance { get; private set; }
    public bool IsGameRunning { get => isGameRunning;}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontró más de un administrador de eventos en la escena.");
        }
        instance = this;
    }


    private void Start()
    {

        itemsLevel = new Dictionary<string, bool>();
        FillItemsLevelDictionary();
        ActivateTimer();
        GameEventsManager.instance.onItemCollected += HandleItemCollected; //Recoger Item
        GameEventsManager.instance.onDeadEnemy += HandleDeadEnemy; //Matar un enemigo
        GameEventsManager.instance.onFallPlayer += HandleFallPlayer; //Cae el jugador
        GameEventsManager.instance.onHitEnemy += HandleHitEnemy; //Es golpeado el jugador
        GameEventsManager.instance.onPlayerDeath += HandlePlayerDeath; // El jugador muere
        GameEventsManager.instance.onLevelCompleted += HandleLevelComplete; //Completar el nivel
    }


    void Update()
    {
        if (endTime)
        {
            UpdateTimer();
        }

        if (IsGameRunning)
        {
            levelTime += Time.deltaTime;
        }
    }



    //Método
    private void FillItemsLevelDictionary()
    {
        itemCount = collection.transform.childCount;

        for (int i = 0; i < itemCount; i++)
        {
            GameObject childObject = collection.transform.GetChild(i).gameObject;
            
            Item itemComponent = childObject.GetComponent<Item>();

            if (itemComponent != null)
            {
                string itemId = itemComponent.Id;
              

                if (!itemsLevel.ContainsKey(itemId))
                {
                    itemsLevel.Add(itemId, false);
                }
            }
        }
    }

    private void HandleHitEnemy(float damage)
    {
        //Debug.Log("Daño recibido: " + damage);

        float realDamage = damage * 2;

        currentTime -= realDamage;
    }
    private void HandlePlayerDeath()
    {
        AudioManager.Instance.StopMusic();
        deathPlayer = true;
        DataPersistenceManager.instance.SaveGameDataOnly();
    }
    private void HandleFallPlayer()
    {
        //Debug.Log("¡El jugador se ha caído");
    }

    //Método que se le llama cuando matas a un enemigo
    private void HandleDeadEnemy()
    {
        deathEnemyCount++;
    }

    // Método llamado cuando se completa el nivel:
    private void HandleLevelComplete()
    {
        DeactivateReaper();
        levelCompleteTransition.SetActive(true);
        Time.timeScale = 0;

        levelSquirrelTransition.AnimateLevelComplete();
        scoreLevel.SetLevelData(levelTime, itemsCollected, deathEnemyCount);
        isGameRunning = false;

        DetectKeyToChangeScene();
    }

    //Método que se le llama cuando recoges un item
    private void HandleItemCollected()
    {
        itemsCollected++;

        /*if (itemsCollected == itemCount)
        {
            Debug.Log("Todos items recogidos");      
        }*/

        if (!objectiveCompleted && itemsCollected >= numItemMinComplete)
        {
            passLevel.CompleteObjective();
            levelSquirrelTransition.RotatePassMessage();
            objectiveCompleted = true;
        }
    }



    private void UpdateTimer()
    {
        currentTime -= Time.deltaTime;

        if(currentTime >= 0)
        {
            sliderTime.value = currentTime;
        }

        if(currentTime <= 0)
        {
            sliderTime.value = currentTime;
            StartCoroutine(ActiveReaper());

            levelSquirrelTransition.ActivateAlarmClock();

            bgFront.SetTrigger("EndTime");
            bgBack.SetTrigger("EndTime");


            AudioManager.Instance.PlaySFX(4);
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic(3);

            //Agregar funcionamiento y código en el futuro
            ChangeTimerState(false);
        }
    }


    public int GetCurrentScore()
    {
        return (itemsCollected * ScoreLevel.pointsPerItemCollected) + (deathEnemyCount * ScoreLevel.pointsPerEnemyDefeated);
    }

    IEnumerator ActiveReaper()
    {
        yield return new WaitForSeconds(timeActivateReaper);
        reaper.SetActive(true);
    }

    public void DeactivateReaper()
    {
        reaper.SetActive(false);
    }

    private void ChangeTimerState(bool estado)
    {
        endTime = estado;
    }


    public void ActivateTimer()
    {
        currentTime = maxTime;
        sliderTime.maxValue = maxTime;
        ChangeTimerState(true);
    }

    public void DesactivateTimer()
    {
        ChangeTimerState(false);
    }






    void DetectKeyToChangeScene()
    {
        DataPersistenceManager.instance.SaveGameDataOnly();
        // Espera a que se presione la tecla "Z" para cambiar de escena
        StartCoroutine(WaitForKeyPress( () =>
        {
            levelSquirrelTransition.FadeOutAndLoadScene("LevelSelection");
            Time.timeScale = 1f;
        }));
    }

    IEnumerator WaitForKeyPress(System.Action action)
    {
        yield return new WaitForSecondsRealtime(4f);

        while (true)
        {
            //Debug.Log("Esperando");

            if (InputManager.GetInstance().GetSubmitPressed())
            {
                action?.Invoke();
                break;
            }
            yield return null;
        }
    }


    public void UpdateItemCollectedStatus(string itemId, bool collectedStatus)
    {
        if (itemsLevel.ContainsKey(itemId))
        {
            itemsLevel[itemId] = collectedStatus;
        }
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.onItemCollected -= HandleItemCollected;
        GameEventsManager.instance.onDeadEnemy -= HandleDeadEnemy;
        GameEventsManager.instance.onLevelCompleted -= HandleLevelComplete;
    }

    public void LoadData(GameData data)
    {
        currentLevel = data.GetLevelByName(nameCurrentLevel,numCurrentLevel);
        numItemMinComplete = currentLevel.minItems;
        maxTime = currentLevel.time;
    }
    public void SaveData(GameData data)
    {
        if (deathPlayer)
        {
            currentLevel.deaths++;
            return;
        }

        foreach (var kvp in itemsLevel)
        {
            string itemId = kvp.Key;
            bool isCollectedInLevel = kvp.Value;

            if (data.itemsCollected.ContainsKey(itemId) && isCollectedInLevel)
            {
                data.itemsCollected[itemId] = true;
            }
        }

        //Puntaje actual
        int newScore = scoreLevel.TotalScore;

        if (currentLevel != null)
        {
            if (newScore > currentLevel.bestScore)
            {
                currentLevel.bestScore = newScore;
            }
        }
    }
}
