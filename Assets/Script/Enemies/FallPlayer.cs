using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Vector2 normal = Vector2.up; // Puedes ajustar esta dirección según sea necesario
            other.gameObject.GetComponent<PlayerCombat>().takeDamage(3, normal, true);
        }
        Debug.Log("Me caigooo");
    }
}
