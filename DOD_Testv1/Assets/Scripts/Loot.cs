using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

    [System.Serializable]
    public class DropCurrency
    {
        public string name;
        public GameObject item;
        public int dropRarity;
    }

    public List<DropCurrency> Loots = new List<DropCurrency>();
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

            for (int i = 0; i < Loots.Count; i++)
                itemWeight += Loots[i].dropRarity;

            int randomValue = Random.Range(0, itemWeight);

            for (int j = 0; j < Loots.Count; j++)
            {
                if (randomValue <= Loots[j].dropRarity)
                {
                    GameObject Obj = Instantiate(Loots[j].item, transform.position + Drop, Quaternion.identity);
                    if (j == 0)
                        Obj.tag = "Coin";
                    else if (j == 1)
                        Obj.tag = "Money Bag";
                    else if (j == 2)
                        Obj.tag = "Gold Bar";
                    return;
                }
                randomValue -= Loots[j].dropRarity;
            }
        }
    }
}
