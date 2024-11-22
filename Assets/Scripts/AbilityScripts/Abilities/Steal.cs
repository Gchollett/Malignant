using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steal : TriggeredAbility
{
    void Start(){
        owner.staticTriggers.TryAdd(Triggers.OnDamagingPlayer,new List<Action>());
        owner.staticTriggers[Triggers.OnDamagingPlayer].Add(triggeredAction);
    }
    public override void triggeredAction()
    {
        if(owner.status == CardStatus.Protags){
            gm.protag.upPips(gm.antag.lowerPips());
        }else if(owner.status == CardStatus.Antags){
            gm.antag.upPips(gm.protag.lowerPips());
        }
    }
}