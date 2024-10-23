using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinky : StatusEffect
{
    public override void effect()
    {
        if(!gameObject.GetComponent<Creature>()) return;
        gameObject.GetComponent<Creature>().tempPower-=1;
    }
}