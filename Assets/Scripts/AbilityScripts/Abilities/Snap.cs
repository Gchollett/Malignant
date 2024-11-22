using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : ActivatedAbility
{
    public override void activatedAction()
    {
        Lane lane = owner.lane.GetComponent<Lane>();
        if(owner.status == CardStatus.Protags){
            if(lane.antagCreature && lane.antagCreature.GetComponent<CreatureCard>().canBlockDirect){
                lane.antagCreature.GetComponent<CreatureCard>().tempHealth -= owner.power+owner.tempPower;
                owner.ActivateTrigger(Triggers.OnDealingDamage);
            }else{
                gm.antag.damage(owner.power+owner.tempPower);
                owner.ActivateTrigger(Triggers.OnDamagingPlayer);
            }
        }else if(owner.status == CardStatus.Antags){
            if(lane.protagCreature && lane.protagCreature.GetComponent<CreatureCard>().canBlockDirect){
                lane.protagCreature.GetComponent<CreatureCard>().tempHealth -= owner.power+owner.tempPower;
                owner.ActivateTrigger(Triggers.OnDealingDamage);
            }else{
                gm.protag.damage(owner.power+owner.tempPower);
                owner.ActivateTrigger(Triggers.OnDamagingPlayer);
            }
        }
    }
}