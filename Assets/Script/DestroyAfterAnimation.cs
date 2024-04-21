using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public float destroyDelay = 0.8f;

    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}
