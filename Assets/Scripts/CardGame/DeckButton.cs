using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckButton : MonoBehaviour
{
    private CardGameManager gm;
    public Deck deck;

    private void Start() {
        gm = CardGameManager.Instance;
    }

    private IEnumerator DrawCardInGame()
    {
        gameObject.GetComponent<Animator>().SetBool("Drawing",false);
        gameObject.GetComponent<Animator>().SetBool("Adding",true);
        yield return new WaitUntil(() => gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Exit") && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
        gm.protag.AddGameCard(deck.draw(),-1);
        gameObject.GetComponent<Animator>().SetBool("Adding",false);
        transform.position = new Vector3(10,0,0);
    }

    private void OnMouseDown() {
        if(!gm.isDrawEnabled) return;
        StartCoroutine(DrawCardInGame());
        foreach(Lane lane in gm.lanes){
            if(lane.protagCreature)lane.protagCreature.GetComponent<Card>().ActivateTrigger(Triggers.OnDraw);
            if(lane.antagCreature)lane.antagCreature.GetComponent<Card>().ActivateTrigger(Triggers.OnDraw);
        }
        gm.changePhase();
    }
}
