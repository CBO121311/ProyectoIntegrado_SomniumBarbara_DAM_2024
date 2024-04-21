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
        // base case - if the profileId is null, return right away
        if (profileId == null)
        {
            return null;
        }



        //Utilice path.combine para tener en cuenta los diferentes sistemas operativos que tienen diferentes separadores de ruta
        string fullPath = Path.Combine(dataDirPath, profileId,dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {

                //Load the serialized data from the file
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
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



                //deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }catch (Exception e)
            {
                Debug.Log("Error ocurre cuando intentas cargar los datos del file" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    //Guardar Datos
    public void Save(GameData data, string profileId)
    {
        // base case - if the profileId is null, return right away
        if (profileId == null)
        {
            return;
        }

        //usamos el Path.Combine para los distintos sistemas operativos.
        string fullPath = Path.Combine(dataDirPath, profileId,dataFileName);
        try
        {
            //Crear directorio si no existe
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serializar el c# game data object a Json
            string dataToStore = JsonUtility.ToJson(data,true);

            //Opcionalmente encriptamos los datos
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //escribe los datos serializados en el archivo

            using (FileStream stream = new FileStream(fullPath,FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }


        }catch (Exception e)
        {
            Debug.LogError($"Error ocurre cuando intentas crear save data en {fullPath} \n {e}");
        }

    }


    //Borrar datos
    public void Delete(string profileId)
    {
        //base case - if the profileId is null, return right away
        if(profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            //ensure the data file exists at this path before deleting the directory
            if (File.Exists(fullPath))
            {
                //delete the profile folder and everything within it
                Directory.Delete(Path.GetDirectoryName(fullPath),true);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }


        }catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data for profileId: " + profileId + " at path: " + fullPath);
        }
    }


    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<String, GameData> profileDictionary = new Dictionary<string, GameData>();

        //loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            //defensive programming - check if the data file exists
            // if it doesn't, then this folder isn't a profile and should be skipped
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + 
                    profileId);
                continue;
            }

            //Load the game data for this profile and put it in the dictionary.
            GameData profileData = Load(profileId);
            // defensive programming - ensure the profile data isn't null,
            // because if it is then something went wrong and we should let ourselves know
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);

            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileID: " + profileId);
                //Debug.Log("Tried to load profile but something went wrong. ProfileID: " + profileId);
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

            //Skip this entry if the gamedata is null
            if (gameData == null)
            {
                continue;
            }

            //if this is the first data we've come across that exists, it's the most recent so far
            if(mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;   
            }

            //otherwhise, compare to see which data is the most recent
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

    //the below is a simple implemntation of xor encryption
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
