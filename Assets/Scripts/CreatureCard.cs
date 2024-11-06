using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class CreatureCard : Card
{
    public int power;
    public int health;
    public string flavorText;
    public Ability ab1;
    public Ability ab2;
    public Ability ab3;
    private GameObject lane;
    private bool position_found = false;
    private bool card_locked = false;
    private int card_index;
    public int tempHealth {get; set;}
    public int tempPower {get; set;}
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
    private void OnMouseDown() {
        if(card_locked) return;
        card_index = gameObject.transform.GetSiblingIndex();
        CardGameManager.Instance.protag.RemoveGameCard(card_index);
    }
    private void OnMouseDrag() {
        if(card_locked) return;
        Plane dragPlane = new Plane(Camera.main.transform.forward, transform.position);
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;
        if (dragPlane.Raycast(camRay, out enter)) 
        {
            Vector3 mousePosition = camRay.GetPoint(enter);
            transform.position = mousePosition;
        }
    }
    private void OnMouseUp()
    {
        if(position_found){
            Vector2 tgtPos = lane.GetComponent<Lane>().playerPt;
            lane.GetComponent<Lane>().cardInLane();
            transform.position = new Vector2(tgtPos.x,tgtPos.y - 1);
            card_locked =  true;
            position_found = true;
        }else{
            CardGameManager.Instance.protag.AddGameCard(gameObject,card_index);
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag == "Card Snappable" && !position_found && !other.gameObject.GetComponent<Lane>().alreadyHasCard()){
            position_found = true;
            lane = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Card Snappable" && position_found){
            position_found = false;
            lane = null;
        }
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
