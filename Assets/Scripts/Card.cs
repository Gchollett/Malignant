using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public string rarity;
    public abstract void display();
    public static CardGameManager gm;
    private GameObject lane;

    private bool position_found = false;
    private bool card_locker = false;

    void Start()
    {
        gm = CardGameManager.Instance;
    }
    private void OnMouseDrag() {
        if(card_locker) return;
        //gm.protag.Hand.Remove(gameObject);
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
        if(position_found && lane){
            Vector2 tgtPos = lane.transform.position;
            transform.position = new Vector2(tgtPos.x,tgtPos.y - 1);
            lane.gameObject.GetComponent<Lane>().cardInLane();
            // gm.protag.Hand.
            card_locker =  true;
            position_found = true;
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag == "Card Snappable" && !position_found){
            Debug.Log($"Snapping to Object: {other.gameObject.name}");
            position_found = true;
            lane = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Card Snappable"){
            other.gameObject.GetComponent<Lane>().cardRemovedFromLane();
        }
    }
    
     
}
