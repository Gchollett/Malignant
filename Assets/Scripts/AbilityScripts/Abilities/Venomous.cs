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
        if(lane.protagCreature == owner.gameObject){
            lane.antagCreature?.GetComponent<CreatureCard>().Kill();
        }else if(lane.antagCreature == owner.gameObject){
            lane.protagCreature?.GetComponent<CreatureCard>().Kill();
        }
    }
}