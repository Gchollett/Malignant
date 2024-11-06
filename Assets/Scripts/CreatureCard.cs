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
    private GameObject lane;
    private bool position_found = false;
    private bool card_locked = false;
    private int card_index;

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
