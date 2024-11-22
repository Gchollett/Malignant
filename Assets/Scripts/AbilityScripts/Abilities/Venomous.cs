using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venomous : TriggeredAbility
{
    void Start(){
        owner.staticTriggers.TryAdd(Triggers.OnDealingDamage,new List<Action>());
        owner.staticTriggers[Triggers.OnDealingDamage].Add(triggeredAction);
    }
    public override void triggeredAction()
    {
        Lane lane = owner.lane.GetComponent<Lane>();
        if(owner.status == CardStatus.Protags){
            lane.antagCreature?.GetComponent<CreatureCard>().Poison();
        }else if(owner.status == CardStatus.Antags){
            lane.protagCreature?.GetComponent<CreatureCard>().Poison();
        }
    }
}