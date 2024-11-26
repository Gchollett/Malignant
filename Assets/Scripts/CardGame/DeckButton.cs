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
        foreach(Lane lane in gm.lanes){
            lane.protagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnDraw);
            lane.antagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnDraw);
        }
        gm.changePhase();
    }
}
