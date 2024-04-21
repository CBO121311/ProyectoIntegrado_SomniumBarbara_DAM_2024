using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaOpciones : MonoBehaviour
{
    // Start is called before the first frame update
    public ControladorOpciones panelOption;

    void Start()
    {
        panelOption = GameObject.FindGameObjectWithTag("Options").GetComponent<ControladorOpciones>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            MostrarOpciones();   
        }   
    }

    public void MostrarOpciones()
    {
        panelOption.pantallaOpciones.SetActive(true);
    }
}
