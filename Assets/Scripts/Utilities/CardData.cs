using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data", menuName = "Scriptable Objects/Card Data", order = 1)]
public class CardData : ScriptableObject
{
    public string cardName;
    public string rarity;
    public int power;
    public int health;
    public string flavorText;
    public List<Ability> abilities;
    public Sprite image;
}
