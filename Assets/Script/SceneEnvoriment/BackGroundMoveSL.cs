using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMoveSL : MonoBehaviour
{
    public float moveSpeedX;
    public float moveSpeedY;
    private Material material;


    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        float offsetX = moveSpeedX * Time.deltaTime;
        float offsetY = moveSpeedY * Time.deltaTime;

        material.mainTextureOffset += new Vector2(offsetX, offsetY);
    }
}
