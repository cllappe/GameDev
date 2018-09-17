using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]

public class Card : ScriptableObject {

    public string cardName;
    public string discription;

    public Sprite art;

    public bool isPowerUp;
    public bool isAttack;
    public bool isHeal;

    //Variables for Attack card
    public int damage;
    public int numberOfTargets;

    //Variables for Heal card
    public int heal;

    //Variables for Power Ups
    public int numOfTurns;
    public int addLuck;
    public int increamDmg;
    public int increasedDef;
    public int playableAmountIncrease;
    public int draw;
}
