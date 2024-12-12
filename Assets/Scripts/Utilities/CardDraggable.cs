using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardDraggable : MonoBehaviour
{
    public Transform canvasTransform;
    protected CardHolder holder;
    protected int originalIndex;
    public Hunter hunter;
    public CardData card;
    protected Transform originalParent;
    private void Start() {
        originalIndex = transform.GetSiblingIndex();
        originalParent = transform.parent.transform;
    }
    private void OnMouseDown() {
        transform.SetParent(canvasTransform);
        if(!holder){
            hunter.deck.Remove(card);
        }
        else if(holder.card == card){
            holder.card=null;
        }
    }
    private void OnMouseDrag() {
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
        if(holder && !holder.card && holder.active){
            transform.SetParent(holder.transform);
            transform.position = transform.parent.position;
            holder.card = card;
        }else{
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalIndex);
            hunter.deck.Add(card);
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(holder)return;
        CardHolder temp_holder;
        other.gameObject.TryGetComponent(out temp_holder);
        holder = temp_holder;
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(!holder || !other.gameObject.GetComponent<CardHolder>())return;
        holder = null;
    }
}
