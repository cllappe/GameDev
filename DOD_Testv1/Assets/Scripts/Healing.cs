using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour {

    [System.Serializable]
    public class DropHealing
    {
        public string name;
        public GameObject item;
        public int dropRarity;
    }

    public List<DropHealing> healings = new List<DropHealing>();
    public int dropChance;

    void calculateDrop()
    {
        int calcDropChance = Random.Range(0, 101);
        Vector3 Drop = new Vector3(0, .3f, -1);
        if (calcDropChance > dropChance)
            return;

        if (calcDropChance <= dropChance)
        {
            int itemWeight = 0;

            for (int i = 0; i < healings.Count; i++)
                itemWeight += healings[i].dropRarity;

            int randomValue = Random.Range(0, itemWeight);

            for (int j = 0; j < healings.Count; j++)
            {
                if (randomValue <= healings[j].dropRarity)
                {
                    GameObject Obj = Instantiate(healings[j].item, transform.position + Drop, Quaternion.identity);
                    if (j == 0)
                        Obj.tag = "Carrot";
                    else if (j == 1)
                        Obj.tag = "Grapes";
                    else if (j == 2)
                        Obj.tag = "Steak";
                    return;
                }
                randomValue -= healings[j].dropRarity;
            }
        }
    }
}
