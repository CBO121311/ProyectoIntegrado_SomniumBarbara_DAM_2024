using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float startWaitTime;
    private float waitedTime;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        waitedTime = startWaitTime;
    }

    void Update()
    {
        // Restablecer el tiempo
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            waitedTime = startWaitTime;
        }


        // Si se mantiene presionada la tecla
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
    
            if (waitedTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitedTime = startWaitTime;
            }
            else
            {
                waitedTime -= Time.deltaTime;
            }
        }

        // Restablecer la rotación
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            effector.rotationalOffset = 0; 
        }
    }
}