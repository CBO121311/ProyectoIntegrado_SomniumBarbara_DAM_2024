using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;

    }

    //Cargar datos 
    public GameData Load(string profileId)
    {

        if (profileId == null)
        {
            return null;
        }



        //Utilice path.combine para tener en cuenta los diferentes sistemas operativos que tienen diferentes separadores de ruta
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {

                
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //Opcionalmente encriptamos los datos
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }



                
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.Log("Error ocurre cuando intentas cargar los datos del file" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    //Guardar Datos
    public void Save(GameData data, string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        //usamos el Path.Combine para los distintos sistemas operativos.
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            //Crear directorio si no existe
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serializar el c# game data object a Json
            string dataToStore = JsonUtility.ToJson(data, true);

            //Opcionalmente encriptamos los datos
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //escribe los datos serializados en el archivo

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }


        }
        catch (Exception e)
        {
            Debug.LogError($"Error ocurre cuando intentas crear save data en {fullPath} \n {e}");
        }

    }


    //Borrar datos
    public void Delete(string profileId)
    {
        //Si el ID del perfil es nulo, regresa de inmediato
        if (profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            //Se comprueba que existe el archivo
            if (File.Exists(fullPath))
            {
                //Eliminar la carpeta del perfil y todo lo que contiene
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Se intentó eliminar los datos del perfil, pero no se encontraron datos en la ruta: " + fullPath);
            }


        }
        catch (Exception e)
        {
            Debug.LogError("No se pudieron eliminar los datos del perfil para perfilId profileId: " + profileId + " con path: " + fullPath);
        }
    }


    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<String, GameData> profileDictionary = new Dictionary<string, GameData>();

        //recorre todos los nombres de directorio 
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            //comprobar si el archivo de datos existe
            // si no es así, entonces esta carpeta no es un perfil y debe omitirse
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " +
                    profileId);
                continue;
            }

            //Cargue los datos del juego para este perfil y póngalos en el diccionario.
            GameData profileData = Load(profileId);

          
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);

            }
            else
            {
                Debug.LogError("Se intentó cargar el perfil pero algo salió mal. ProfileID: " + profileId);
            }
        }

        return profileDictionary;
    }


    //Para el continue
    public string getMostRecentlyUpdateProfile()
    {
        string mostRecentProfileId = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
            {
                continue;
            }

       
            if (mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }

            //Compare para ver qué datos son los más recientes
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                //the greatest DateTime value us the most recent
                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }
        }

        return mostRecentProfileId;
    }

    //Implementación del cifrado xor para el guardado.
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
