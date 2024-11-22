using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hop : ActivatedAbility
{
    public override void activatedAction()
    {
        owner.setDirectDamage(true);
        owner.tempTriggers.TryAdd(Triggers.OnEnd,new List<(Action,int)>());
        owner.tempTriggers[Triggers.OnEnd].Add((delegate () {owner.setDirectDamage(false);},1));
    }
}