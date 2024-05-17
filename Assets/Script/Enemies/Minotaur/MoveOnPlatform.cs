using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveOnPlatform : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float velocidadDeMovimiento;
    public LayerMask capaAbajo;
    public LayerMask capaEnfrente;

    public float distanciaAbajo;
    public float distanciaEnfrente;

    public Transform controladorAbajo;
    public Transform controladorEnfrente;
    public bool informacionAbajo;
    public bool informacionEnfrente;

    private bool mirandoAlaDerecha = true;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(velocidadDeMovimiento, rb2D.velocity.y);
        informacionEnfrente = Physics2D.Raycast(controladorEnfrente.position, transform.right, distanciaEnfrente, capaEnfrente);

        //informacionAbajo = Physics2D.Raycast(controladorAbajo.position, Vector2.down, distanciaAbajo, capaAbajo);
        informacionAbajo= Physics2D.Raycast(controladorAbajo.position, transform.up * -1, distanciaAbajo, capaAbajo);

        /*if (informacionEnfrente)
        {
            Debug.Log("¡Hay un obstáculo frente al objeto!");
        }
        else
        {
            Debug.Log("¡No hay obstáculo!");
        }

        if (!informacionAbajo)
        {
            Debug.Log("¡No hay suelo debajo del objeto!");
        }
        else
        {
            Debug.Log("¡Hay suelo!");
        }*/



        if (informacionEnfrente || !informacionAbajo)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoAlaDerecha = !mirandoAlaDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        velocidadDeMovimiento *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorAbajo.transform.position, controladorAbajo.transform.position + Vector3.down * distanciaAbajo);
    

        //Gizmos.DrawLine(controladorAbajo.transform.position, controladorAbajo.transform.position + transform.up * -1 *distanciaAbajo);
        Gizmos.DrawLine(controladorEnfrente.transform.position, controladorEnfrente.transform.position + transform.right * -1 * distanciaEnfrente);
    }
}
