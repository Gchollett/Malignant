using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TESTING : MonoBehaviour
{

    private static CardGameManager gm;
    public StatusEffect se;
    public Ability ab;

    void Start()
    {
        gm = CardGameManager.Instance;
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"Apply {(se?se.effectName:ab.adjective)}";
    }

    public void applyStatusEffect(){
        gm.lanes[0].protagCreature?.GetComponent<CreatureCard>().applyStatusEffect(se,3);
    }

    public void applyAbility(){
        if(gm.protag.Hand.Count > 0) gm.protag.gameObject.transform.GetChild(0).GetComponent<CreatureCard>().addAbility(ab,true);
    }
}
