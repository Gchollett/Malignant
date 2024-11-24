using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckButton : MonoBehaviour
{
    private CardGameManager gm;
    public Deck deck;

    private void Start() {
        gm = CardGameManager.Instance;
    }

    private void OnMouseDown() {
        if(!gm.isDrawEnabled) return;
        gm.protag.AddGameCard(deck.draw(),-1);
        gm.changePhase();
    }
}
