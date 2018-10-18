using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName ="Character")]

public class Character : ScriptableObject{
    public enum Type
    {
        PLAYER,
        ENEMY,
        MINIBOSS,
        BOSS
    }

    public Type charType;
    public Sprite avatar;
    public int health;
    public int luck;
    public int basicAttackDmg;
    public GameObject healthBar;
    public bool dmgReflected = false;
    public float defence;
    public int dmgMod;
    public bool skipTurn = false;

}
