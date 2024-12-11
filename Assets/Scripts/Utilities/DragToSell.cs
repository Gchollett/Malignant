using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class DragToSell : Draggable{
    public int cost {get; set;}
    private static DataManager dm;
    public AbilityStore ab {get;set;}
    private void Start() {
        dm = DataManager.Instance;
        originalIndex = transform.GetSiblingIndex();
        originalParent = transform.parent.transform;
    }
    private void OnMouseUp() {
        if(card && dm.money>=cost){
            card.addAbility(transform.GetChild(0).GetComponent<AbilityPreview>().ab);
            dm.money -= cost;
            ab.StoreAbilities.Remove((gameObject,cost));
            Destroy(gameObject);
        }else{
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalIndex);
        }
        
    }
}