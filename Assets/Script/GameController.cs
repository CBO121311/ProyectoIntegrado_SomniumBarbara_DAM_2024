using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IDataPersistence
{

    [Header("Timer")]
    [SerializeField] private float limitTime;
    [SerializeField] private Slider sliderTime;
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

        //AudioManager.instance.PlayMusic("SquirrelMusic");


        itemsLevel = new Dictionary<string, bool>();
        FillItemsLevelDictionary();
        ActivarTemporizador();
        GameEventsManager.instance.onItemCollected += HandleItemCollected; //Recoger Item
        GameEventsManager.instance.onDeadEnemy += HandleDeadEnemy; //Matar un enemigo
        GameEventsManager.instance.onFallPlayer += HandleFallPlayer; //Cae el jugador
        GameEventsManager.instance.onHitEnemy += HandleHitEnemy; //Es golpeado el jugador
        GameEventsManager.instance.onPlayerDeath += HandlePlayerDeath; // El jugador muere
        GameEventsManager.instance.onLevelCompleted += HandleLevelComplete; //Completar el nivel
    }
    private void HandleHitEnemy(float damage)
    {
        //Debug.Log("Daño recibido: " + damage);

        float realDamage = damage * 10;

        currentTime -= realDamage;
    }
    private void HandlePlayerDeath()
    {
        deathPlayer = true;
        DataPersistenceManager.instance.SaveGameDataOnly();
        SceneManager.LoadScene("LevelSelection");
    }



    private void HandleFallPlayer()
    {
        Debug.Log("¡El jugador se ha caído");
    }

    void Update()
    {
        if (endTime)
        {
            CambiarContador();
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

 

    private void CambiarContador()
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

            //mainAudioSource.PlayOneShot(AudioManager.instance.GetAudioClip("AlarmSound"));
            AudioManager.instance.PlayOneSound("AlarmSound");
            AudioManager.instance.ChangeMusicTerror();


            //Agregar funcionamiento y código en el futuro
            CambiarTemporador(false);
        }
    }

    IEnumerator ActiveReaper()
    {
        yield return new WaitForSeconds(10f);
        reaper.SetActive(true);
    }

    private void CambiarTemporador(bool estado)
    {
        endTime = estado;
    }


    public void ActivarTemporizador()
    {
        //tiempoActual = tiempoMaximo +1;
        currentTime = limitTime;
        sliderTime.maxValue = limitTime;
        CambiarTemporador(true);
    }

    public void DesactivarTemporizador()
    {
        CambiarTemporador(false);
    }

    //Método que se le llama cuando matas a un enemigo
    private void HandleDeadEnemy()
    {
        deathEnemyCount++;
    }


    // Método llamado cuando se completa el nivel:
    // - Desactiva al enemigo Reaper.
    // - Activa la transición de finalización del nivel.
    // - Pausa el tiempo en el juego.
    // - Realiza la animación de finalización del nivel.
    // - Establece los datos del nivel en la pantalla de puntuación.
    // - Detiene cualquier acción del juego.
    // - Detecta la tecla para cambiar de escena al siguiente nivel.
    private void HandleLevelComplete()
    {
        DeactivateReaper();
        levelCompleteTransition.SetActive(true);
        Time.timeScale = 0;

        levelSquirrelTransition.AnimateLevelComplete();
        scoreLevel.SetLevelData(levelTime,itemsCollected,deathEnemyCount);
        isGameRunning = false;

        DetectKeyToChangeScene();
    }


    void DetectKeyToChangeScene()
    {
        DataPersistenceManager.instance.SaveGameDataOnly();
        // Espera a que se presione la tecla "Z" para cambiar de escena
        StartCoroutine(WaitForKeyPress( () =>
        {
            SceneManager.LoadScene("LevelSelection");
            Time.timeScale = 1f;
        }));
    }

    IEnumerator WaitForKeyPress(System.Action action)
    {
        yield return new WaitForSecondsRealtime(4f);

        while (true)
        {
            Debug.Log("Esperando");

            if (InputManager.GetInstance().GetSubmitPressed())
            {
                action?.Invoke();
                break;
            }
            yield return null;
        }
    }


    //Método que se le llama cuando recoges un item
    private void HandleItemCollected()
    {
        itemsCollected++;

        if (itemsCollected == itemCount)
        {
            Debug.Log("¡Todos los artículos han sido recolectados!");      
        }

        if (itemsCollected >= 5)
        {
            Debug.Log("Se han recolectado al menos 5 elementos.");
            passLevel.CompleteObjective();
            levelSquirrelTransition.RotatePassMessage();
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
        //throw new System.NotImplementedException();
    }
    public void SaveData(GameData data)
    {
        Level currentLevel = data.informationLevel.Find(level => level.name == "Ardilla");

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
                // Actualizar el valor en itemsCollected a verdadero
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



    public void DeactivateReaper()
    {
        reaper.SetActive(false);
    }
}
