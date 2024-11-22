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


public class CreatureCard : Card
{
    [SerializeField]
    private int power;
    [SerializeField]
    private int health;
    public string flavorText;
    public Ability ab1;
    public Ability ab2;
    public Ability ab3;
    public List<Ability> abilities {get; set;}
    public GameObject lane {get; set;}
    private bool position_found = false;
    private bool card_locked = false;
    private int card_index;
    public int tempHealth {get; set;}
    public int tempPower {get; set;}
    public int extraAttackCounter {get; set;}
    public bool isAttackStopped {get; set;}
    private bool dying;
    public bool isDealingDirect {get; private set;} //Boolean for that allows the creature to avoid attacking opposing creatures
    private static CardGameManager gm;
    private GameObject prefab;
    public Dictionary<StatusEffect,int> statusEffects; //The status effect and the duration of it
    public Dictionary<Triggers,List<(Action,int)>> tempTriggers {get; set;} //The triggers that only need to trigger a finite number of times
    public Dictionary<Triggers,List<Action>> staticTriggers {get;set;} //The triggers that should trigger until removed
    public Button abilityButton1; 
    public Button abilityButton2; 
    public Button abilityButton3; 
    private Vector3 initialScale;
    public float scale = 1.5f;
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

    void Start()
    {
        initialScale = transform.localScale;
        abilities = new List<Ability>
        {
            ab1,
            ab2,
            ab3
        };

        tempTriggers = new Dictionary<Triggers, List<(Action, int)>>();
        staticTriggers = new Dictionary<Triggers, List<Action>>();
        statusEffects = new Dictionary<StatusEffect, int>();
        gm = CardGameManager.Instance;
        if(ab1){
            ab1 = Instantiate(ab1,transform);
            ab1.owner = this;
        }
        if(ab2){
            ab2 = Instantiate(ab2,transform);
            ab2.owner = this;
        }
        if(ab3){
            ab3 = Instantiate(ab3,transform);
            ab3.owner = this;
        }
    }
    void FixedUpdate() {
        abilities = new List<Ability>
        {
            ab1,
            ab2,
            ab3
        };
        setCardText();
        if(power + tempPower < 0){
            tempPower = -power;
        }
    }

    private void setCardText(){
        string adjectives = "";
        foreach (StatusEffect se in statusEffects.Keys){
            if(statusEffects[se] > 0) adjectives = se.effectName + " " + adjectives;
        }
        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = adjectives + cardName;
        transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = ((power + tempPower >= 0)?(power + tempPower):0).ToString();
        transform.GetChild(2).gameObject.GetComponent<TextMeshPro>().text = (health + tempHealth).ToString();
        if(ab1){
            transform.GetChild(3).gameObject.GetComponent<TextMeshPro>().text = ab1.abilityName;
            if(ab1 is ActivatedAbility){
                abilityButton1.gameObject.SetActive(true);
                abilityButton1.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{((ActivatedAbility)ab1).cost} pips";
            }
        } else {
             transform.GetChild(3).gameObject.GetComponent<TextMeshPro>().text = "";
        }

        if(ab2){
            transform.GetChild(4).gameObject.GetComponent<TextMeshPro>().text = ab2.abilityName;
            if(ab2 is ActivatedAbility){
                abilityButton2.gameObject.SetActive(true);
                abilityButton2.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{((ActivatedAbility)ab2).cost} pips";
            }
        } else {
             transform.GetChild(4).gameObject.GetComponent<TextMeshPro>().text = "";
        }

        if(ab3){
            transform.GetChild(5).gameObject.GetComponent<TextMeshPro>().text = ab3.abilityName;
            if(ab3 is ActivatedAbility){
                abilityButton3.gameObject.SetActive(true);
                abilityButton3.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{((ActivatedAbility)ab3).cost} pips";
            }
        } else {
             transform.GetChild(5).gameObject.GetComponent<TextMeshPro>().text = "";
        }
    }

    public void attack(Player p){
        p.damage(power+tempPower<0?0:power+tempPower);
    }
    public void attack(CreatureCard c){
        c.tempHealth -= power+tempPower<0?0:power+tempPower;
    }
    public void Kill()
    {
        if(dying)return;
        dying = true;
        ActivateTrigger(Triggers.OnDeath);
        lane.GetComponent<Lane>().removeFromLane(gameObject);
        Destroy(gameObject);
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
        CheckIfDead();
    }

    public void CheckIfDead(){
        if(health + tempHealth <= 0){
            Kill();
        }
    }

    private void OnMouseEnter(){
        transform.localScale *= scale;
    }

    private void OnMouseExit(){
        transform.localScale = initialScale;
    }

    
    private void OnMouseDown() {
        if(card_locked || !gm.isMoveEnabled) return;
        card_index = gameObject.transform.GetSiblingIndex();
        prefab = CardGameManager.Instance.protag.RemoveGameCard(card_index);
    }
    private void OnMouseDrag() {
        if(card_locked || !gm.isMoveEnabled) return;
        Plane dragPlane = new Plane(Camera.main.transform.forward, transform.position);
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;
        if (dragPlane.Raycast(camRay, out enter)) 
        {
            Vector3 mousePosition = camRay.GetPoint(enter);
            transform.position = mousePosition;
        }
        transform.localScale = initialScale;
    }
    private void OnMouseUp()
    {
        if(!gm.isMoveEnabled) return;
        if(position_found){
            lane.GetComponent<Lane>().addProtagCreature(gameObject);
            card_locked =  true;
            position_found = true;
            gm.changeActivePlayer();
        }else if(prefab){
            gm.protag.AddGameCard(prefab,card_index);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag == "Card Snappable" && !card_locked && !position_found && !other.gameObject.GetComponent<Lane>().alreadyHasCard()){
            position_found = true;
            lane = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Card Snappable" && !card_locked && position_found){
            position_found = false;
            lane = null;
        }
    }

    public void addAbility(Ability ab){
        if(!ab1){
            ab1 = Instantiate(ab);
            ab1.owner = this;
            cardName = ab.adjective+ " " + cardName;
        }else if(!ab2){
            ab2 = Instantiate(ab);
            ab2.owner = this;
            cardName = ab.adjective+ " " + cardName;
        }else if(!ab3){
            ab3 = Instantiate(ab);
            ab3.owner = this;
            cardName = ab.adjective+ " " + cardName;
        }
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

    public void setDirectDamage(bool value){
        Debug.Log($"Setting Direct Damage to {value}");
        isDealingDirect = value;
    }
}
