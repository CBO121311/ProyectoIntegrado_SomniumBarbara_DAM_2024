using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character
{
    public string name;
    public bool canDoubleJump;
    public bool canDealDamage;

    public Character(string name, bool canDoubleJump, bool canDealDamage)
    {
        this.name = name;
        this.canDoubleJump = canDoubleJump;
        this.canDealDamage = canDealDamage;
    }
}