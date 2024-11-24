using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    [SerializeField] private CreatureCard creature;    
    private CardGameManager gm;
    private bool card_locked;
    private bool position_found;
    private int card_index;
    private GameObject prefab;
    void Start()
    {
        gm = CardGameManager.Instance;
    }
    private void OnMouseDown() {
        if(creature.status == CardStatus.Protags && gm.isSacrificeEnabled){
            creature.Sacrifice();
        }else{
            if(card_locked || !gm.isMoveEnabled) return;
            card_index = gameObject.transform.GetSiblingIndex();
            prefab = CardGameManager.Instance.protag.RemoveGameCard(card_index);
        }
    }
    private void OnMouseDrag() {
        if(card_locked || !gm.isMoveEnabled || creature.status != CardStatus.Unplayed) return;
        Plane dragPlane = new Plane(Camera.main.transform.forward, transform.position);
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;
        if (dragPlane.Raycast(camRay, out enter)) 
        {
            Vector3 mousePosition = camRay.GetPoint(enter);
            transform.position = mousePosition;
        }
        transform.localScale = creature.initialScale;
    }
    private void OnMouseUp()
    {
        if(!gm.isMoveEnabled || creature.status != CardStatus.Unplayed) return;
        if(position_found && creature.status == CardStatus.Unplayed){
            creature.lane.GetComponent<Lane>().addProtagCreature(gameObject);
            card_locked =  true;
            position_found = true;
            gm.changeActivePlayer();
            creature.status = CardStatus.Protags;
            gm.protag.fixHand(true);
            creature.ActivateTrigger(Triggers.OnEnter);
        }else if(prefab){
            gm.protag.AddGameCard(prefab,card_index);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag == "Card Snappable" && !card_locked && !position_found && creature.status != CardStatus.Antags && !other.gameObject.GetComponent<Lane>().alreadyHasCard() && transform.localScale == creature.initialScale){
            position_found = true;
            creature.lane = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Card Snappable" && !card_locked && position_found && creature.status != CardStatus.Antags  && transform.localScale == creature.initialScale){
            position_found = false;
            creature.lane = null;
        }
    }
}
