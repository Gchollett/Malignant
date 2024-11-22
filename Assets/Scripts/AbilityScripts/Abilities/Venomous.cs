using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venomous : TriggeredAbility
{
    void Start(){
        owner.tempTriggers.TryAdd(Triggers.OnDeath,new List<(Action,int)>());
        owner.tempTriggers[Triggers.OnDeath].Add((triggeredAction,1));
    }
    public override void triggeredAction()
    {
        Lane lane = owner.lane.GetComponent<Lane>();
        if(owner.status == CardStatus.Protags){
            lane.antagCreature?.GetComponent<CreatureCard>().Kill();
        }else if(owner.status == CardStatus.Antags){
            lane.protagCreature?.GetComponent<CreatureCard>().Kill();
        }
    }
}