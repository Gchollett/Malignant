using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public string rarity;
    public abstract void display();

    private void OnMouseDrag() {
        Plane dragPlane = new Plane(Camera.main.transform.forward, transform.position);
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;
        if (dragPlane.Raycast(camRay, out enter)) 
        {
            Vector3 fingerPosition = camRay.GetPoint(enter);
            transform.position = fingerPosition;
        }
    }
}
