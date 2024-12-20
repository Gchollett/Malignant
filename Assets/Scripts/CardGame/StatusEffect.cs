using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    public static CardGameManager gm;
    public string effectName;
    public string description;
    void Start()
    {
        gm = CardGameManager.Instance;
    }
    public abstract void effect(Card creature);
    public abstract void deffect(Card creature);
}
