using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public string rarity;
    public abstract void display();

    private bool position_locked = true;


    private void OnMouseDrag() {
        position_locked = false;

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

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag == "Card Snappable" && !position_locked){
            Debug.Log($"Snapping to Object: {other.gameObject.name}");
            position_locked = true;
            Vector2 tgtPos = other.transform.position;
            transform.position = new Vector2(tgtPos.x,tgtPos.y - 1);
            other.gameObject.GetComponent<Lane>().cardPlayedInLane();
        }
    }
    
}
