using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class GoldSaver : MonoBehaviour {

    private PlayerController pc;

    private void Awake()
    {
        pc = gameObject.GetComponent<PlayerController>();
    }

    public void OnRecordPersistentData()
    {
        DialogueLua.SetActorField("Player", "gold", pc.Gold);
    }

    public void OnApplyPersistentData()
    {
        //pc.Gold = DialogueLua.GetActorField("Player", "gold").asInt;
    }
}
