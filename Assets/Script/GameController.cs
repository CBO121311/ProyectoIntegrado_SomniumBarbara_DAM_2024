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
    [SerializeField] private GameObject reaper;
    [SerializeField] private Animator bgFront;
    [SerializeField] private Animator bgBack;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioPersecution;

    private void Start()
    {
        ActivarTemporizador();
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
            bgFront.SetTrigger("EndTime");
            bgBack.SetTrigger("EndTime");
            audioSource.Stop();
            audioSource.clip = audioPersecution;
            audioSource.Play();
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
}
