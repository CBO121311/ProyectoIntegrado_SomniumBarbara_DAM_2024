using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private float tiempoMaximo;
    [SerializeField] private Slider slider;
    private float tiempoActual;
    private bool tiempoActivado = false;
    [SerializeField] private GameObject reaper;
    [SerializeField] private Animator bgFront;
    [SerializeField] private Animator bgBack;
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioSource soundAudioSource;
    [SerializeField] private AudioClip clockclip;
    [SerializeField] private AudioClip audioPersecution;
    [SerializeField] private LevelTransition levelTransition;

    public GameObject transition;
    public GameObject collection;
    

    private Dictionary<string, bool> itemsLevel;

    //Número total de items
    private int itemCount;
    //Items recogidos
    private int itemsCollected = 0;
    public int ItemCount { get => itemCount;}
    public int ItemsCollected { get => itemsCollected;}

    private void Start()
    {
        itemsLevel = new Dictionary<string, bool>();
        FillItemsLevelDictionary();
        ActivarTemporizador();
        GameEventsManager.instance.onItemCollected += OnItemCollected;
    }
    void Update()
    {
        if (tiempoActivado)
        {
            CambiarContador();
        }
    }

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
        tiempoActual -= Time.deltaTime;

        if(tiempoActual >= 0)
        {
          
            /*int minutos = Mathf.FloorToInt(tiempoActual / 60);
            int segundos = Mathf.FloorToInt(tiempoActual % 60);
            string tiempoFormateado = $"{minutos:00}:{segundos:00}";
            Debug.Log($"Tiempo restante: {tiempoFormateado}");*/

            slider.value = tiempoActual;
        }

        if(tiempoActual <= 0)
        {
            Debug.Log("Derrota");
            StartCoroutine(ActiveReaper());

            levelTransition.ActivateAlarmClock();

            bgFront.SetTrigger("EndTime");
            bgBack.SetTrigger("EndTime");

            soundAudioSource.PlayOneShot(clockclip);

            mainAudioSource.Stop();
            mainAudioSource.clip = audioPersecution;
            mainAudioSource.Play();

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
        tiempoActivado = estado;
    }


    public void ActivarTemporizador()
    {
        //tiempoActual = tiempoMaximo +1;
        tiempoActual = tiempoMaximo;
        slider.maxValue = tiempoMaximo;
        CambiarTemporador(true);
    }

    public void DesactivarTemporizador()
    {
        CambiarTemporador(false);
    }

    private void OnItemCollected()
    {
        itemsCollected++;

        if (itemsCollected == itemCount)
        {
            Debug.Log("¡Todos los artículos han sido recolectados!");
            transition.SetActive(true);
        }
        else
        {
            Debug.Log("Quedan " + (itemCount - itemsCollected) + " artículos por recolectar.");
        }

        if (itemsCollected >= 5)
        {
            Debug.Log("Se han recolectado al menos 5 elementos.");
            Invoke("ChangeScene", 1);
        }
    }

    public void UpdateItemCollectedStatus(string itemId, bool collectedStatus)
    {
        if (itemsLevel.ContainsKey(itemId))
        {
            itemsLevel[itemId] = collectedStatus;
        }
    }

    void ChangeScene()
    {
        Debug.Log("Enhorabuena, te lo has pasado");
        DataPersistenceManager.instance.SaveGame();
       
        Invoke("LoadNextScene", 1f);
    }
    void LoadNextScene()
    {
        //SceneManager.LoadScene(1);
    }


    private void OnDestroy()
    {
        GameEventsManager.instance.onItemCollected -= OnItemCollected;
    }

    public void LoadData(GameData data)
    {
        //throw new System.NotImplementedException();
    }
    public void SaveData(GameData data)
    {
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
    }
}
