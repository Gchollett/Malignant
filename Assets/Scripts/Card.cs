using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public string rarity;
    public abstract void display();
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
        //Movement Handling, Prolly not optimal
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
            Debug.Log($"{lane} found, attempting to snap to tgtPos");
            Vector2 tgtPos = lane.GetComponent<Lane>().playerPt;
            transform.position = new Vector2(tgtPos.x,tgtPos.y - 1);
            lane.gameObject.GetComponent<Lane>().cardInLane();
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
    
     
}
