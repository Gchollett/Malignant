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
        if(pips < cost) return false;
        activatedAction();
        
        return true;
    }

    public override bool ProcessAbility()
    {
        return false;
    }

    public override bool ProcessAbility(string val)
    {
        return false;
    }
}