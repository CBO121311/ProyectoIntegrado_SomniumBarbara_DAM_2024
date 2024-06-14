using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{

    private int weaponDamage = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("LE DI WEAPON COLLIDER");

            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {

                Vector2 contactPoint = other.ClosestPoint(transform.position);
                Vector2 pushDirection = ((Vector2)other.transform.position - contactPoint).normalized;
                Vector2 pushVector = pushDirection * new Vector2(-2, -2); // Magnitud ajustada

                playerCombat.TakeDamage(weaponDamage, pushVector);
            }
        }
    }
}
