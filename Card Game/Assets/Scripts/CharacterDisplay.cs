using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterDisplay : MonoBehaviour {
    public Image avatar;
    public int health;
    public Character.Type characterType;
    public int charLuck;
    public int basicAttack;
    private Character myCharacter;
    public GameObject healthBarGO;
    public bool dmgWillBeReflected;
    public float defence;
    public int dmgMod;
    public GameObject CBText;

    public void characterSetup(Character thisCharacter){
        myCharacter = thisCharacter;
        avatar.sprite = myCharacter.avatar;
        health = myCharacter.health;
        characterType = myCharacter.charType;
        charLuck = myCharacter.luck;
        basicAttack = myCharacter.basicAttackDmg;
        healthBarGO = myCharacter.healthBar;
        dmgWillBeReflected = myCharacter.dmgReflected;
        defence = myCharacter.defence;
        dmgMod = myCharacter.dmgMod;
        CBText = myCharacter.combatDMGText;
    }
}
