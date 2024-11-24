using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : MonoBehaviour{

    public List<GameObject> cards;
    private DataManager dm;

    void Start()
    {
        dm = DataManager.Instance;
        if(dm) cards = new List<GameObject>(dm.Deck);
    }
    public GameObject draw(){
        if(cards.Count == 0) return null;
        GameObject card = cards[0];
        cards.RemoveAt(0);
        return card;
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
}
