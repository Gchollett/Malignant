using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class DragToUpgrade : Draggable{
    private void OnMouseUp() {
        if(card){
            card.addAbility(transform.GetComponent<AbilityPreview>().ab);
            GameObject.Find("Pop-up Canvas").transform.GetChild(1).gameObject.SetActive(false);
        }else{
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalIndex);
        }
        
    }
}