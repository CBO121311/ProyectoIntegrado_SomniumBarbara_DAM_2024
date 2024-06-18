using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTime : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.DesactivateTimer();
        }
    }
}
