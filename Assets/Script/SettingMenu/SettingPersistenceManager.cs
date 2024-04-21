using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPersistenceManager : MonoBehaviour
{
    private void Awake()
    {
        var noDestruirEntrEscenas = FindObjectsOfType<SettingPersistenceManager>();
        if (noDestruirEntrEscenas.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
