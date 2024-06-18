using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    bool _isGrounded = false;
    Rigidbody2D rb;

    void Start(){
        rb = GetComponentInParent<Rigidbody2D>();
    }

    public bool IsGrounded{
        get{
            return _isGrounded;
        }
    }

    void OnTriggerEnter2D(Collider2D other){

        if (other.CompareTag("Ground") && rb.velocityY <= 0)
        {
            _isGrounded = true;
        }

    }

    void OnTriggerStay2D(Collider2D other){

        if (other.CompareTag("Ground") && rb.velocityY <= 0)
        {
            _isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){

        if (other.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}
