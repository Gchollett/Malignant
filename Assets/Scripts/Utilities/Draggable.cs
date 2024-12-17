using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public Transform ParentTransform;
    protected Card card;
    protected int originalIndex;
    protected Transform originalParent;
    private void Start() {
        originalIndex = transform.GetSiblingIndex();
        originalParent = transform.parent.transform;
    }
    private void OnMouseDown() {
        transform.SetParent(ParentTransform);
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
        if(card){
            card.addAbility(gameObject.GetComponent<AbilityPreview>().ab);
        }
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalIndex);
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(card)return;
        Card temp_card;
        other.gameObject.TryGetComponent(out temp_card);
        if(temp_card)other.transform.localScale = Vector3.one*3;
        card = temp_card;
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(!card || !other.gameObject.GetComponent<Card>())return;
        other.transform.localScale = Vector3.one*2;
        card = null;
    }
}
