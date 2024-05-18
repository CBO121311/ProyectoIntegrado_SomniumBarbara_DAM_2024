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
            Invoke("OutPlayer", 0.3f);
        }
    }

    void OutPlayer()
    {
        GameEventsManager.instance.PlayerDeath();
    }
}
