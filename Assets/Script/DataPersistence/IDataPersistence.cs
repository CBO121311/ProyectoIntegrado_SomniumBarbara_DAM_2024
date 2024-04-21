using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data);

    //Se utiliza ref para modificar los datos
    void SaveData(GameData data);
}
