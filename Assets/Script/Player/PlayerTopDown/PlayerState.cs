using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static bool newGame = false, otherDay = false, wakeUp = false, nightmare = false;

    public static void Allfalse()
    {
        newGame = false;
        otherDay = false;
        wakeUp = false;
        nightmare = false;
    }
}
