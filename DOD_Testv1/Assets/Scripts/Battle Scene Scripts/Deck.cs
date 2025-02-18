﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Make Deck")]
public class Deck : ScriptableObject {
    public int deckID;
    public Card[] cards;
}