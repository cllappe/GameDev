using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour {
    public Image avatar;
    public int health;

    private Character myCharacter;

    public void characterSetup(Character thisCharacter){
        myCharacter = thisCharacter;
        avatar.sprite = myCharacter.avatar;
        health = myCharacter.health;
    }
}
