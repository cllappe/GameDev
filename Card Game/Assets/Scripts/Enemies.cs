using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Array",menuName = "Create Enemy Array")]
public class Enemies : ScriptableObject
{
    public Character[] enemies;
}
