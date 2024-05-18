using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    private void Start()
    {
        if (TemporaryData.UseTemporaryPosition)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
            {
                player.transform.position = TemporaryData.PlayerPosition;
                TemporaryData.UseTemporaryPosition = false;

                Debug.Log("Mover posición del jugador");
            }
        }
        Debug.Log("Player: " + TemporaryData.PlayerPosition);
    }
}
