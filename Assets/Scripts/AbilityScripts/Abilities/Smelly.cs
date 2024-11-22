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
        if(lane.protagCreature == owner.gameObject){
            lane.antagCreature?.GetComponent<CreatureCard>().applyStatusEffect(StinkyPrefab,2);
        }else if(lane.antagCreature == owner.gameObject){
            lane.protagCreature?.GetComponent<CreatureCard>().applyStatusEffect(StinkyPrefab,2);
        }
    }
}
