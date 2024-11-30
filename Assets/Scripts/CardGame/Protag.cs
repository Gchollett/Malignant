using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protag : Player {
    private void FixedUpdate() {
        fixHand();
    }

    public GameObject RemoveGameCard(int i) {
        GameObject ret = Hand[i];
        Hand.RemoveAt(i);
        gameObject.transform.GetChild(i).transform.SetParent(CardGameManager.Instance.gameObject.transform);
        return ret;
    }

    public void AddGameCard(GameObject card,int index){
        Hand.Insert(index,card);
        // card.transform.SetParent(gameObject.transform);
        // card.transform.SetSiblingIndex(index);
    }

    private void fixHand() {
        if(Hand.Count != transform.childCount){
            for(int i = 0; i < transform.childCount; i++){
                Destroy(transform.GetChild(i).gameObject);
            }
            for(int i = 0; i < Hand.Count; i++){
                GameObject card = Instantiate(Hand[i]);
                card.transform.parent = gameObject.transform;
                card.transform.position = gameObject.transform.position + new Vector3(2*(i-Hand.Count/2f+.5f),0,0);
            }
        }else{
            for(int i = 0; i < Hand.Count; i++){
                transform.GetChild(i).gameObject.transform.position = gameObject.transform.position + new Vector3(2*(i-Hand.Count/2f+.5f),0,0);
            }
        }
        
    }
}