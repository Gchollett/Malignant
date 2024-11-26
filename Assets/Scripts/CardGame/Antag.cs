using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Antag : Player {    
    Card playCard(Lane lane){
        CardData card  = Hand[Random.Range(0,Hand.Count)];
        GameObject creature = Instantiate(CardPrefab,transform);
        creature.GetComponent<Card>().cardData = card;
        lane.addAntagCreature(creature);
        creature.GetComponent<Card>().lane = lane.gameObject;
        creature.GetComponent<Card>().status = CardStatus.Antags;
        Hand.Remove(card);
        return creature.GetComponent<Card>();
    }
   public void MakePlay(Lane[] lanes){
        if(Hand.Count != 0){
            Card playedCard = null;
            List<Lane> emptyLanes = new List<Lane>();
            foreach(Lane lane in lanes){
                if(!playedCard && lane.protagCreature && !lane.antagCreature){
                    playedCard = playCard(lane);
                }else if(!lane.antagCreature){
                    emptyLanes.Add(lane);
                }
            }if(!playedCard){
                if(emptyLanes.Count > 0){
                    playedCard = playCard(emptyLanes[Random.Range(0,emptyLanes.Count)]);
                }
            }
            playedCard?.ActivateTrigger(Triggers.OnEnter);
        }
    }

    public void activateAbilities(Lane[] lanes)
    {
        foreach(Lane lane in lanes){ //Go through and activate debuffs
            if(lane.antagCreature && lane.antagCreature.GetComponent<Card>().abilities.Count > 0){
                Card creature = lane.antagCreature.GetComponent<Card>();
                foreach(Ability ab in creature.abilities){
                    float prob = 0.01f;
                    switch(ab){
                        case Scare:
                            prob = .05f;
                            if(pips >= ((ActivatedAbility)ab).cost+2) prob = .3f;
                            break;
                    }
                    if(Random.Range(0f,1f) > prob) if(ab.ProcessAbility(pips)){lowerPips(((ActivatedAbility)ab).cost);};
                }
            }            
        }
        foreach(Lane lane in lanes){ //Go through and activate buffs
            if(lane.antagCreature && lane.antagCreature.GetComponent<Card>().abilities.Count > 0){
                Card creature = lane.antagCreature.GetComponent<Card>();
                foreach(Ability ab in creature.abilities){
                    float prob = 0.01f;
                    switch(ab){
                        case Honk:
                            prob = .6f;
                            break;
                        case Hop:
                            prob = .7f;
                            break;                            
                        case Rage:
                            if(creature.getInGameHealth() > 1){
                                if(lane.protagCreature){
                                    Card protagCreature = lane.protagCreature.GetComponent<Card>();
                                    if(protagCreature.isBlockStopped || protagCreature.getInGamePower() < creature.getInGameHealth()-1 || protagCreature.getInGameHealth() <= creature.getInGamePower()+2){
                                        prob = .5f;
                                    }
                                }else{
                                    prob = .7f;
                                }
                            }
                            break;
                        default: 
                            prob = 0;
                            break;
                    }
                    if(Random.Range(0f,1f) > prob) if(ab.ProcessAbility(pips)){lowerPips(((ActivatedAbility)ab).cost);};
                }
            }            
        }
        foreach(Lane lane in lanes){ //Go through and activate Damage
            if(lane.antagCreature && lane.antagCreature.GetComponent<Card>().abilities.Count > 0){
                Card creature = lane.antagCreature.GetComponent<Card>();
                foreach(Ability ab in creature.abilities){
                    float prob = 0.01f;
                    switch(ab){
                        case Hasten:
                            if(lane.protagCreature){
                                if(creature.getInGamePower() * 2 >= lane.protagCreature.GetComponent<Card>().getInGameHealth()){
                                    prob = .7f;
                                }
                            }else{
                                if(creature.getInGamePower() > 1){
                                    prob = .8f;
                                }else if(creature.getInGamePower() == 1){
                                    prob = .2f;
                                }
                            }
                            break;
                        case Bonk:
                            if(lane.protagCreature && lane.protagCreature.GetComponent<Card>().getInGameHealth() <= creature.getInGamePower()*1.5){
                                prob = .5f;
                            }else{
                                prob = 0;
                            }
                            break;
                        case Snap:
                            if(lane.protagCreature){
                                prob = .6f;
                            }else{
                                prob = .2f;
                            }
                            break;
                        default: 
                            prob = 0;
                            break;
                    }
                    if(Random.Range(0f,1f) > prob) if(ab.ProcessAbility(pips)){lowerPips(((ActivatedAbility)ab).cost);};
                }
            }            
        }
        foreach(Lane lane in lanes){ //Go through and activate Misc
            if(lane.antagCreature && lane.antagCreature.GetComponent<Card>().abilities.Count > 0){
                Card creature = lane.antagCreature.GetComponent<Card>();
                foreach(Ability ab in creature.abilities){
                    float prob = 0.01f;
                    switch(ab){
                        case PlayDead:
                            if(lane.protagCreature){
                                Card protagCreature = lane.protagCreature.GetComponent<Card>();
                                foreach(Ability abi in creature.abilities){
                                    if((abi is Honk || abi is Jam || abi is Snap) && (protagCreature.getInGamePower() * (protagCreature.extraAttackCounter+1) >= creature.getInGameHealth() || protagCreature.hasAbility(typeof(Venomous))) && protagCreature.getInGamePower() < health/2){
                                        prob = .7f;
                                    }
                                }
                            }
                            break;
                        default: 
                            prob = 0;
                            break;
                    }
                    if(Random.Range(0f,1f) > prob) if(ab.ProcessAbility(pips)){lowerPips(((ActivatedAbility)ab).cost);};
                }
            }            
        }
    }
}