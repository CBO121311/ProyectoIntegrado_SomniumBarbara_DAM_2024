using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TestData : MonoBehaviour
{
    [SerializeField] private Image imagenObjeto;
    [SerializeField] private string id;

    public int vitality;
    public int stregth;
    public int intellect;
    public int endurace;

    public TestData()
    {
        this.vitality = 1;
        this.stregth = 1;
        this.intellect = 1; 
        this.endurace = 1;
    }

    
}
