using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ActivatedAbility : Ability
{
    public int cost;
    public abstract void activatedAction();
    public override bool ProcessAbility(int pips){
        if(pips < cost || owner.isAbilitiesStopped) return false;
        activatedAction();
        owner.ActivateTrigger(Triggers.OnActivate);
        return true;
    }

    public override bool ProcessAbility()
    {
        return false;
    }
}