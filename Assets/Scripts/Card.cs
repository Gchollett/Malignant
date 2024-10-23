using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public string rarity;
    public abstract void display();
}
