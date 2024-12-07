using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : TriggeredAbility
{
    void Start(){
        owner.tempTriggers.TryAdd(Triggers.OnSacrifice,new List<(Action,int)>());
        owner.tempTriggers[Triggers.OnSacrifice].Add((triggeredAction,1));
    }
    public override void triggeredAction()
    {
        if(owner.status == CardStatus.Protags){
            gm.protag.upPips();
        }else if(owner.status == CardStatus.Antags){
            gm.antag.upPips();
        }
    }
}