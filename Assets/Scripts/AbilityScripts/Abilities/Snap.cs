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
            if(lane.antagCreature && lane.antagCreature.GetComponent<Card>().canBlockDirect){
                lane.antagCreature.GetComponent<Card>().tempHealth -= owner.cardData.power+owner.tempPower;
                owner.ActivateTrigger(Triggers.OnDealingDamage);
            }else{
                gm.antag.damage(owner.cardData.power+owner.tempPower);
                owner.ActivateTrigger(Triggers.OnDamagingPlayer);
            }
        }else if(owner.status == CardStatus.Antags){
            if(lane.protagCreature && lane.protagCreature.GetComponent<Card>().canBlockDirect){
                lane.protagCreature.GetComponent<Card>().tempHealth -= owner.cardData.power+owner.tempPower;
                owner.ActivateTrigger(Triggers.OnDealingDamage);
            }else{
                gm.protag.damage(owner.cardData.power+owner.tempPower);
                owner.ActivateTrigger(Triggers.OnDamagingPlayer);
            }
        }
    }
}