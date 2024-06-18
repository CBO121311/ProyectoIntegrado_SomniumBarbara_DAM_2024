using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;
    [SerializeField] private BoxCollider2D playerCollider;

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }*/


    public void CheckAndDisableCollision()
    {
        if (currentOneWayPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }
    // Update is called once per frame
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
