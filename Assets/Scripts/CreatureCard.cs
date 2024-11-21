using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;


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
    public GameObject lane {get; set;}
    private bool position_found = false;
    private bool card_locked = false;
    private int card_index;
    public int tempHealth {get; set;}
    public int tempPower {get; set;}
    private static CardGameManager gm;
    private GameObject prefab;
    public Dictionary<StatusEffect,int> statusEffects; //The status effect and the duration of it
    public float scale = 1.5f;
    public float inspectScale = 5f;
    public GameObject inspectPos;
    public void Apply(StatusEffect se,int duration){
        if(statusEffects.ContainsKey(se)){
            statusEffects[se] += duration;
        }else{
            statusEffects[se] = duration;
            se.effect(this);
        }
    }

    void Start()
    {
        statusEffects = new Dictionary<StatusEffect, int>();
        gm = CardGameManager.Instance;
    }
    void FixedUpdate() {
        setCardText();
    }

    private void setCardText(){
        string adjectives = "";
        foreach (StatusEffect se in statusEffects.Keys){
            if(statusEffects[se] > 0) adjectives = se.effectName + " " + adjectives;
        }
        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = adjectives + cardName;
        transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = ((power + tempPower >= 0)?(power + tempPower):0).ToString();
        transform.GetChild(2).gameObject.GetComponent<TextMeshPro>().text = (health + tempHealth).ToString();
        if(ab1 != null){
            transform.GetChild(3).gameObject.GetComponent<TextMeshPro>().text = ab1.abilityName;
        } else {
             transform.GetChild(3).gameObject.GetComponent<TextMeshPro>().text = "";
        }

        if(ab2 != null){
            transform.GetChild(4).gameObject.GetComponent<TextMeshPro>().text = ab2.abilityName;
        } else {
             transform.GetChild(4).gameObject.GetComponent<TextMeshPro>().text = "";
        }

        if(ab3 != null){
            transform.GetChild(5).gameObject.GetComponent<TextMeshPro>().text = ab3.abilityName;
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
        ab1?.ProcessAbility("Death");
        ab2?.ProcessAbility("Death");
        ab3?.ProcessAbility("Death");
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
        if(health + tempHealth <= 0){
            Kill();
        }
        if(power + tempPower < 0){
            tempPower = -power;
        }
    }

    private void OnMouseEnter(){
        transform.localScale *= scale;
    }

    private void OnMouseExit(){
        transform.localScale /= scale;
    }

    private void InspectCard(){
        transform.localScale *= inspectScale;
        transform.position = inspectPos.transform.position;
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
