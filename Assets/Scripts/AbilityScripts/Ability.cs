using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public static CardGameManager gm;
    public CreatureCard owner {get; set;}
    void Start()
    {
        gm = CardGameManager.Instance;
    }
    public string abilityName;
    public string description;
    public string adjective;
    public abstract bool ProcessAbility();
    public abstract bool ProcessAbility(int pips);
    public abstract bool ProcessAbility(string val);
}
