using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{



    /*private GameObject player;
    private BoxCollider2D ccPlayer;
    private BoxCollider2D ccPlata;
    private Bounds ccPlataBounds;
    private Vector2 ccPlayerSize;
    private Color colorGizmos = Color.yellow;
    private float topPlata, piePlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ccPlayer = player.GetComponent<BoxCollider2D>();
        ccPlata = GetComponent<BoxCollider2D>();
        ccPlataBounds = ccPlata.bounds;
        ccPlayerSize = ccPlayer.size;
        topPlata = ccPlataBounds.center.y + ccPlataBounds.extents.y; //tope hacia arriba.
    }

    // Update is called once per frame
    void Update()
    {
        piePlayer = player.transform.position.y - ccPlayer.size.y /2;
        if (piePlayer > topPlata) 
        { 
         ccPlata.isTrigger = false;
            gameObject.tag = "OneWayPlatform";
            Debug.Log("False");
        }
    }*/
}
