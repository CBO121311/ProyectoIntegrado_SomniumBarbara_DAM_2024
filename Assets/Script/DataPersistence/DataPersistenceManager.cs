using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


public class DataPersistenceManager : MonoBehaviour
{

    //Para hacer partidas de pruebas
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;

    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTimeSeconds = 60f;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = ""; //test2

    //AutoGuardado
    private Coroutine autoSaveCoroutine;
    public static DataPersistenceManager instance { get; private set; }

    //Seguimiento de los datos
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Se encontró más de un administrador de persistencia de datos en la escena. Destruyendo el más nuevo.");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence está deshabilitado");
        }


        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }

    private void OnEnable()
    {
        //Suscribirse al evento.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Cuando se cargue una escena reiniciamos nuestra lista de objeto de persistencia de datos.
        //Y luego cargamos el juego.
        //Debug.Log("OnSceneLoaded");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();


        //Arrancar el autosave coroutine
        /*
        if(autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }

        autoSaveCoroutine = StartCoroutine(AutoSave());*/
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        //actualizar el perfil para usarlo para guardar y cargar
        this.selectedProfileId = newProfileId;
        //Cargar el juego, que utilizará ese perfil, actualizando nuestros datos del juego en consecuencia.
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        //eliminar los datos de esta identificación de perfil
        dataHandler.Delete(profileId);

        //inicializar la identificación del perfil seleccionado
        InitializeSelectedProfileId();

        //Vuelva a cargar el juego para que nuestros datos coincidan con la nueva identificación del perfil seleccionado.
        LoadGame();
    }
    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.getMostRecentlyUpdateProfile();

        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Override selected profile id with test id: " + testSelectedProfileId);
        }
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }


    public void LoadGame()
    {
        //regresar de inmediato si la persistencia de datos está deshabilitada
        if (disableDataPersistence)
        {
            return;
        }


        //Cargue cualquier archivo guardado desde un archivo usando el controlador de datos
        this.gameData = dataHandler.Load(selectedProfileId);

        //nicie un nuevo juego si los datos son nulos y estamos configurados para inicializar datos con fines de depuración.
        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }


        //Si no se pueden cargar datos, inicialice a un nuevo juego

        if (this.gameData == null)
        {
            Debug.Log("No se encontraron datos. Es necesario iniciar un nuevo juego antes de poder cargar los datos.");
            return;

        }

        //TODO - envíe los datos cargados a todos los demás scripts que los necesiten
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        //Debug.Log("Loaded death count = " + gameData.deathCount);
    }

    public void SaveGame()
    {
        //regresar de inmediato si la persistencia de datos está deshabilitada
        if (disableDataPersistence)
        {
            return;
        }

        //Si no tenemos ningún dato para guardar, registre una advertencia aquí.

        if (this.gameData == null)
        {
            Debug.LogWarning("No se encontraron datos. Es necesario iniciar un nuevo juego antes de poder guardar los datos.");
        }

        //pasar los datos a otros scripts para que puedan actualizarlos.
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        //marcar la hora de los datos para que sepamos cuándo se guardaron por última vez
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        //guarde esos datos en un archivo utilizando el controlador de datos.
        dataHandler.Save(gameData, selectedProfileId);
        Debug.Log("Saved death count = " + gameData.deathCount);
    }




    private void OnApplicationQuit()
    {
        //SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistencesObjects);

    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }


    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            //SaveGame();
            AutoSaveGame();
            Debug.Log("Auto Saved Game");
        }
    }

    public void AutoSaveGame()
    {
        if (disableDataPersistence)
        {
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("No se encontraron datos. Es necesario iniciar un nuevo juego antes de poder guardar los datos.");
        }
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        Debug.Log("Perfil es " + selectedProfileId);
        dataHandler.Save(gameData, "AutoSave");
    }

}
