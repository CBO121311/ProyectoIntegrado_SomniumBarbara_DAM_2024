using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Muerto");
            ControllerPlayerSquirrel Squirrelplayer = collision.GetComponent<ControllerPlayerSquirrel>();
            if (Squirrelplayer != null)
            {
                Squirrelplayer.Die();
                GameEventsManager.instance.PlayerDeath();
            }
        }
    }
}
