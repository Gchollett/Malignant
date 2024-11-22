using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Deck : MonoBehaviour{

    public List<GameObject> cards;
    private CardGameManager gm;

    void Start()
    {
        gm = CardGameManager.Instance;
    }
    public void draw(){
        if(cards.Count == 0) return;
        gm.protag.Hand.Add(cards[0]);
        cards.RemoveAt(0);
    }

    public void shuffle(){
        var count = cards.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = Random.Range(i, count);
			var tmp = cards[i];
			cards[i] = cards[r];
			cards[r] = tmp;
		}
    }

    void OnMouseDown() {
        if(gm.isDrawEnabled){
            draw();
            gm.changePhase();
        }
    }
}
