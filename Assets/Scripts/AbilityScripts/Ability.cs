using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public static CardGameManager gm;
    public Card owner {get; set;}
    void FixedUpdate()
    {
        gm = CardGameManager.Instance;
    }
    public string abilityName;
    public string description;
    public string adjective;
    public abstract bool ProcessAbility();
    public abstract bool ProcessAbility(int pips);
}
