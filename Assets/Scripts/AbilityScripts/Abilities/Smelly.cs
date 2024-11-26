using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelly : TriggeredAbility
{
    StatusEffect StinkyPrefab;
    void Start()
    {
        StinkyPrefab = Resources.Load<StatusEffect>("Prefabs/StatusEffects/Stinky");
        owner.tempTriggers.TryAdd(Triggers.OnDeath,new List<(Action,int)>());
        owner.tempTriggers[Triggers.OnDeath].Add((triggeredAction,1));
    }
    public override void triggeredAction()
    {
        Lane lane = owner.lane.GetComponent<Lane>();
        if(owner.status == CardStatus.Protags){
            lane.antagCreature?.GetComponent<Card>().applyStatusEffect(StinkyPrefab,2);
        }else if(owner.status == CardStatus.Antags){
            lane.protagCreature?.GetComponent<Card>().applyStatusEffect(StinkyPrefab,2);
        }
    }
}
