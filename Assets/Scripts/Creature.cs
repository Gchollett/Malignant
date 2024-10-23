using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Creature : CreatureCard
{
    [HideInInspector]
    public int laneNumber;
    [HideInInspector]
    public int tempHealth;
    [HideInInspector]
    public int tempPower;
    public List<(StatusEffect,int)> statusEffects; //The status effect and the duration of it
    public void Apply(StatusEffect se,int duration){
        statusEffects.Append((se,duration));
    }

    void Start()
    {
        statusEffects = new List<(StatusEffect, int)>();
    }
    void FixedUpdate() {
        string adjectives = "";
        foreach ((StatusEffect,int) se in statusEffects){
            adjectives = se.Item1 + " " + adjectives;
        }
        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = adjectives + cardName;
        transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = ((power + tempPower >= 0)?(power + tempPower):0).ToString();
        transform.GetChild(2).gameObject.GetComponent<TextMeshPro>().text = (health + tempHealth).ToString();
    }
    public void attack(Player p){
        p.damage(power);
    }
    public void attack(Creature c){
        c.tempHealth -= power;
    }
    public void Kill()
    {
        ab1.ProcessAbility("Death");
        ab2.ProcessAbility("Death");
        ab3.ProcessAbility("Death");
        Destroy(gameObject);
    }
    public void UpdateCard()
    {
        for(int i = 0; i < statusEffects.Count; i++){
            if(statusEffects[i].Item2 > 0){
                statusEffects[i] = (statusEffects[i].Item1,statusEffects[i].Item2-1);
            }else{
                statusEffects.RemoveAt(i);
            }
        }
        if(health + tempHealth <= 0){
            Kill();
        }
    }

    public Creature(CreatureCard card){
        cardName = card.cardName;
        flavorText = card.flavorText;
        power = card.power;
        health = card.health;
        ab1 = card.ab1;
        ab2 = card.ab2;
        ab3 = card.ab3;
    }
}
