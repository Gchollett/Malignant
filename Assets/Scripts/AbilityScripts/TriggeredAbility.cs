using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggeredAbility : Ability
{
    public abstract bool trigger(string val);
    public abstract void triggeredAction();
    public override bool ProcessAbility(string val){
        if(!trigger(val)) return false;
        triggeredAction();
        return true;
    }
    public override bool ProcessAbility()
    {
        return false;
    }

    public override bool ProcessAbility(int pips)
    {
        return false;
    }
}
