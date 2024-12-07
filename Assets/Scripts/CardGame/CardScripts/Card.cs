using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Card : MonoBehaviour
{
    public CardData cardData;
    public SpriteRenderer spriteRenderer;
    public Image image;
    public string cardName {get; private set;}
    public string rarity {get; private set;}    
    public int power {get; private set;}
    public int health {get; private set;}
    public string flavorText {get; private set;}
    public int abilityLimit = 3;
    public List<Ability> abilities {get; private set;}
    public GameObject lane {get; set;}
    public bool canBlockDirect {get; set;}
    public int tempHealth {get; set;}
    public int tempPower {get; set;}
    public int extraAttackCounter {get; set;}
    public bool isAttackStopped {get; set;}
    public bool isBlockStopped {get;set;}
    public bool isAbilitiesStopped {get; set;}
    public bool isPoisoned {get; set;}
    private bool isDying;
    public bool isDealingDirect {get; set;} //Boolean for that allows the creature to avoid attacking opposing creatures
    private static CardGameManager gm;
    public Dictionary<StatusEffect,int> statusEffects {get; set;} = new Dictionary<StatusEffect, int>(); //The status effect and the duration of it
    public HashSet<StatusEffect> staticEffects {get; set;} = new HashSet<StatusEffect>(); //The status effects that don't need a duration
    public Dictionary<Triggers,List<(Action,int)>> tempTriggers {get; set;} = new Dictionary<Triggers, List<(Action, int)>>();//The triggers that only need to trigger a finite number of times
    public Dictionary<Triggers,List<Action>> staticTriggers {get;set;} = new Dictionary<Triggers, List<Action>>(); //The triggers that should trigger until removed
    public Vector3 initialScale {get; private set;}
    public CardStatus status {get; set;} = CardStatus.Unplayed;

    void Start()
    {
        cardName = cardData.cardName;
        power = cardData.power;
        health = cardData.health;
        rarity = cardData.rarity;
        flavorText = cardData.flavorText;
        abilities = new List<Ability>(cardData.abilities);
        if(spriteRenderer) spriteRenderer.sprite = cardData.image;
        else image.sprite = cardData.image;
        initialScale = transform.localScale;
        gm = CardGameManager.Instance;
        for(int i = 0; i < Math.Min(abilities.Count,abilityLimit); i++){
            if(abilities[i]){
                abilities[i] = Instantiate(abilities[i],transform);
                abilities[i].owner = this;
            }
        }
        if(status == CardStatus.Antags){ //Only workaround I found for on enter trigger for antag creatures with the new CardData
            ActivateTrigger(Triggers.OnEnter);
        }
    }
    void FixedUpdate() {
        if(power + tempPower < 0){
            tempPower = -power;
        }
    }

    public void applyStaticEffect(StatusEffect se){
        if(staticEffects.Add(se))se.effect(this);
    }
    public void unapplyStaticEffect(StatusEffect se){
        if(staticEffects.Remove(se)){
            se.deffect(this);
        }
    }
    public void applyStatusEffect(StatusEffect se,int duration){
        if(statusEffects.ContainsKey(se) && statusEffects[se] != 0){
            statusEffects[se] += duration;
        }else{
            statusEffects[se] = duration;
            se.effect(this);
        }
    }

    public void removeStatusEffect(StatusEffect se){
        if(statusEffects.ContainsKey(se) && statusEffects[se] != 0){
            statusEffects[se] = 0;
            se.deffect(this);
        }
    }

    public void attack(Player p){
        p.damage(power+tempPower<0?0:power+tempPower);
    }
    public void attack(Card c){
        c.tempHealth -= power+tempPower<0?0:power+tempPower;
    }
    public void Kill()
    {
        if(isDying)return;
        isDying = true;
        ActivateTrigger(Triggers.OnDeath);
        lane.GetComponent<Lane>().removeFromLane(gameObject);
        Destroy(gameObject);
        abilities.ForEach(ab => {
            Destroy(ab);
        });
    }
    public void UpdateCard()
    {
        foreach(StatusEffect se in new List<StatusEffect>(statusEffects.Keys)){
            if(statusEffects[se] > 0) {
                statusEffects[se] -= 1;
                if(statusEffects[se] == 0){
                    se.deffect(this);
                }
            }
        }
    }

    public void Poison(){
        isPoisoned = true;
    }
    public void CheckIfDead(){
        if(health + tempHealth <= 0 || isPoisoned){
            Kill();
        }
    }    

    public void Sacrifice(){
        if(status == CardStatus.Protags){
            gm.protag.upPips();
            gm.disableSacrifice();            
        }else if(status == CardStatus.Antags){
            gm.antag.upPips();
        }
        ActivateTrigger(Triggers.OnSacrifice);
        lane.GetComponent<Lane>().removeFromLane(gameObject);
        Destroy(gameObject);
        abilities.ForEach(ab => {
            Destroy(ab);
        });
    }

    public bool addAbility(Ability ab,bool test = false){
        int indx = 0;
        while(indx < abilities.Count && indx < abilityLimit && abilities[indx] != null){
            indx ++;
        }
        if(indx == abilityLimit) return false;
        if(indx == abilities.Count){
            abilities.Add(null);
        }
        abilities[indx] = Instantiate(ab);
        abilities[indx].owner = this;
        cardData.abilities.Add(ab);
        cardData.cardName = ab.adjective+ " " + cardName;
        cardName = ab.adjective+ " " + cardName;
        return true;
    }
    public void ActivateTrigger(Triggers trig){
        List<(Action,int)> tempTriggerList;
        tempTriggers.TryGetValue(trig,out tempTriggerList);
        List<(Action,int)> elementsToRemove = new List<(Action,int)>();
        if(tempTriggerList != null){
            for(int i =0; i<tempTriggerList.Count; i++){
                if(tempTriggerList[i].Item2 > 0) {
                    tempTriggerList[i].Item1();
                    tempTriggerList[i]= (tempTriggerList[i].Item1,tempTriggerList[i].Item2-1);
                    if(tempTriggerList[i].Item2<=0) elementsToRemove.Add(tempTriggerList[i]);
                }else{
                    elementsToRemove.Add(tempTriggerList[i]);
                }
            }
            for(int i = 0; i<elementsToRemove.Count; i++){
                tempTriggerList.Remove(elementsToRemove[i]);
            }
            tempTriggers[trig] = tempTriggerList;
        }
        tempTriggers.TryGetValue(trig,out tempTriggerList);
        List<Action> TriggerList;
        staticTriggers.TryGetValue(trig,out TriggerList);
        if(TriggerList != null){
            for(int i =0; i<TriggerList.Count; i++){
                TriggerList[i]();
            }
        }
    }

    public int getInGameHealth()
    {
        return health + tempHealth;
    }

    public int getInGamePower()
    {
        return power + tempPower;
    }

    public bool hasAbility(Type abCheck){
        foreach(Ability ab in abilities){
            if(ab.GetType() == abCheck){
                return true;
            }
        }
        return false;
    }
}
