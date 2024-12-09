using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Jam : PassiveAbility
{
    private StatusEffect InspiredPrefab;
    void Start(){
        InspiredPrefab = Resources.Load<StatusEffect>("Prefabs/StatusEffects/Inspired");
        owner.tempTriggers.TryAdd(Triggers.OnDeath,new List<(Action, int)>());
        owner.tempTriggers[Triggers.OnDeath].Add((delegate () {
            for(int i =0; i< gm.lanes.Count(); i++){
                if(owner.status == CardStatus.Protags) gm.lanes[i].protagCreature?.GetComponent<Card>().unapplyStaticEffect(InspiredPrefab);
                else if(owner.status == CardStatus.Antags) gm.lanes[i].antagCreature?.GetComponent<Card>().unapplyStaticEffect(InspiredPrefab);
            }
        }, 1));
    }
    
    void FixedUpdate()
    {
        staticAction();
    }
    public override void staticAction()
    {
        if(!gm)return;
        for(int i =0; i< gm.lanes.Count(); i++){
            if(owner.status == CardStatus.Protags && owner.gameObject != gm.lanes[i].protagCreature) gm.lanes[i].protagCreature?.GetComponent<Card>().applyStaticEffect(InspiredPrefab);
            else if(owner.status == CardStatus.Antags && owner.gameObject != gm.lanes[i].antagCreature) gm.lanes[i].antagCreature?.GetComponent<Card>().applyStaticEffect(InspiredPrefab);
        }
    }
}
