using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum Targets
{
    PLAYER,
    ENEMY
};

public enum Type
{
    HEALCARD,
    POWERUPCARD,
    ATTACKCARD
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]

public class Card : ScriptableObject, IComparable<Card> {
    public int cardID;

    [Header("Standard Info")]
    public string cardName;
    public string discription;
    public Sprite art;
    public Targets targets;
    public Type type;

    [Header("Attack Card Info")]
    public int damage;
    public int numberOfTargets;
    public bool lifeSteal;

    [Header("Heal Card Info")]
    public int heal;

    [Header("Power Up Info")]
    public int numOfTurns;
    public int addLuck;
    public int increaseDamage;
    public float damageReducedBy;
    public int playableAmountIncrease;
    public int draw;
    public bool reflects;

    [Header("Critical Values")] 
    public bool canCrit;
    public int critNumOfTargets;
    public int critHeal;
    public bool critLifeSteal;
    public int critNumOfTurns;
    public int critDmgAdd;
    public bool critSkip;
    public bool critEnemyCritNegate;
    public int critDraw;
    
    
    

    public int CompareTo(Card Other){
        return this.cardName.CompareTo(Other.cardName);
    }
}
