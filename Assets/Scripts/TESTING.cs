using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTING : MonoBehaviour
{

    private static CardGameManager gm;
    public StatusEffect se;

    void Start()
    {
        gm = CardGameManager.Instance;
    }

    public void applyStatusEffect(){
        gm.lanes[0].protagCreature?.GetComponent<CreatureCard>().Apply(se,3);
    }
}
