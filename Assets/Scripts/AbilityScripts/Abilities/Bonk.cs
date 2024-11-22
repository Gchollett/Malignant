using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bonk : ActivatedAbility
{
    StatusEffect SleepyPrefab;
    void Start()
    {
        SleepyPrefab = Resources.Load<StatusEffect>("Prefabs/StatusEffects/Sleepy");
    }
    public override void activatedAction()
    {
        if(owner.status == CardStatus.Protags && owner.lane.GetComponent<Lane>().antagCreature){
            owner.lane.GetComponent<Lane>().antagCreature.GetComponent<CreatureCard>().tempHealth -= (int)(1.5*(owner.power+owner.tempPower));
            owner.ActivateTrigger(Triggers.OnDealingDamage);
        }else if(owner.status == CardStatus.Antags && owner.lane.GetComponent<Lane>().protagCreature){
            owner.lane.GetComponent<Lane>().protagCreature.GetComponent<CreatureCard>().tempHealth -= (int)(1.5*(owner.power+owner.tempPower));
            owner.ActivateTrigger(Triggers.OnDealingDamage);
        }
        owner.applyStatusEffect(SleepyPrefab,2);
    }
}