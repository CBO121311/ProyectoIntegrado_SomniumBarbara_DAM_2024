using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;


public class DataPersistenceManager : MonoBehaviour
{

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
    public static DataPersistenceManager instance { get; private set; }

    //Seguimiento de los datos
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Hay más de un DataPersistenceManager en la escena. Se destruye el actual");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }

    //Recoge datos del gameData para consultar información o pedir información
    public GameData GetGameData()
    {
        return gameData;
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
            Debug.LogWarning("Sustituye el id de perfil seleccionado por el id de prueba: " + testSelectedProfileId);
        }
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }



    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private bool gameLoaded = false;

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Ha pasado en OnSceneLoaded");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        // Carga los datos del juego desde el archivo solo si no se han cargado previamente
        if (!gameLoaded)
        {
            LoadGame();
            gameLoaded = true;
        }
        else
        {
            ReloadGameData();
        }

        if (scene.name == "MainMenuUI")
        {
            gameLoaded = false;
        }
    }

    [SerializeField] private bool initializeDataIfNull = false;
    public void LoadGame()
    {
        //Debug.Log("Estoy en load game de datapersistence.");

        this.gameData = dataHandler.Load(selectedProfileId);

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

        //Envía los datos cargados a todos los demás scripts que los necesiten
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    private void ReloadGameData()
    {
        Debug.Log("Recargando datos del juego en los objetos de la escena.");

        // Envía los datos cargados a todos los demás scripts que los necesiten
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }



    public void SaveGame()
    {
        Debug.Log("Pasando DataPesistenceManager saveGame");
        
        //Si no tenemos ningún dato para guardado.
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
        gameData.lastUpdated = DateTime.Now.ToBinary();

        //guarde esos datos en un archivo utilizando el controlador de datos.
        dataHandler.Save(gameData, selectedProfileId);
    }

    public void SaveGameDataOnly()
    {
        Debug.Log("Pasando DataPesistenceManager saveGameDataOnly");

        // Si no tenemos ningún dato para guardado.
        if (this.gameData == null)
        {
            Debug.LogWarning("No se encontraron datos. Es necesario iniciar un nuevo juego antes de poder guardar los datos.");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
    }


    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // FindObjectsofType toma un booleano opcional para incluir objetos de juego inactivos
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistencesObjects);
    }


    //Comprueba si se tiene datos
    public bool HasGameData()
    {
        if(gameData != null)
        {
            Debug.Log("Hay datos");

        }
        else
        {
            Debug.Log("No hay datos");
        }

        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
