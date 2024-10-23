using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatureCard : Card
{
    public int power;
    public int health;
    public string flavorText;
    public Ability ab1;
    public Ability ab2;
    public Ability ab3;

    void Start() {
        display();
    }
    public override void display(){
        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = cardName;
        transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = power.ToString();
        transform.GetChild(2).gameObject.GetComponent<TextMeshPro>().text = health.ToString();
    }
    public void addAbility(Ability ab){
        if(ab1 == null){
            ab1 = ab;
            cardName = ab.adjective+ " " + cardName;
        }else if(ab2 == null){
            ab2 = ab;
            cardName = ab.adjective+ " " + cardName;
        }else if(ab3 == null){
            ab3 = ab;
            cardName = ab.adjective+ " " + cardName;
        }
    }
}
