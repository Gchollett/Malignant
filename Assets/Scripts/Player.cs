using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int health {get; private set;}
    public int startingHealth = 10;
    public int pips {get; private set;}
    public List<GameObject> Hand;

    void Start() {
        health = startingHealth;
        pips = 0;
    }
    private void FixedUpdate() {
        fixHand();
    }

    public void RemoveGameCard(int i) {
        Hand.RemoveAt(i);
        gameObject.transform.GetChild(i).transform.SetParent(CardGameManager.Instance.gameObject.transform);
        fixHand();
    }

    public void AddGameCard(GameObject card,int index){
        Debug.Log(index);
        Hand.Insert(index,card);
        card.transform.SetParent(gameObject.transform);
        card.transform.SetSiblingIndex(index);
        fixHand();
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

    public void upPips(int count = 1){
        pips += count;
    }

    public void lowerPips(int count = 1){
        pips -= count;
    }

    public void damage(int d){
        health -= d;
    }
}
