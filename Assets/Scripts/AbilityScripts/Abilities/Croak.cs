using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Croak : TriggeredAbility
{
    StatusEffect TrippinPrefab;
    void Start(){
        TrippinPrefab = Resources.Load<StatusEffect>("Prefabs/StatusEffects/Trippin");
        owner.tempTriggers.TryAdd(Triggers.OnDeath,new List<(Action,int)>());
        owner.tempTriggers[Triggers.OnDeath].Add((triggeredAction,1));
    }
    public override void triggeredAction()
    {
        Lane lane = owner.lane.GetComponent<Lane>();
        if(owner.status == CardStatus.Protags){
            lane.antagCreature?.GetComponent<CreatureCard>().applyStatusEffect(TrippinPrefab,2);
        }else if(owner.status == CardStatus.Antags){
            lane.protagCreature?.GetComponent<CreatureCard>().applyStatusEffect(TrippinPrefab,2);
        }
    }
}