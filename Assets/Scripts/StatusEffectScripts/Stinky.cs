using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinky : StatusEffect
{
    public override void effect()
    {
        if(!gameObject.GetComponent<CreatureCard>()) return;
        gameObject.GetComponent<CreatureCard>().tempPower-=1;
    }
}