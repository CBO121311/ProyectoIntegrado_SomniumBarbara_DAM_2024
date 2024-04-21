using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private float tiempoMaximo;
    [SerializeField] private Slider slider;
    private float tiempoActual;
    private bool tiempoActivado = false;


    private void Start()
    {
        //ActivarTemporizador();
    }

    void Update()
    {
        if (tiempoActivado)
        {
            CambiarContador();
        }
    }


    private void CambiarContador()
    {
        tiempoActual -= Time.deltaTime;

        if(tiempoActual >= 0)
        {
            slider.value = tiempoActual;
        }

        if(tiempoActual <= 0)
        {
            Debug.Log("Derrota");
            //Agregar funcionamiento y código en el futuro
            CambiarTemporador(false);
        }
    }

    private void CambiarTemporador(bool estado)
    {
        tiempoActivado = estado;
    }


    public void ActivarTemporizador()
    {
        tiempoActual = tiempoMaximo;
        slider.maxValue = tiempoMaximo;
        CambiarTemporador(true);
    }

    public void DesactivarTemporizador()
    {
        CambiarTemporador(false);
    }
}
