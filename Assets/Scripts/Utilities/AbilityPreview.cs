using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityPreview : MonoBehaviour
{
    public Ability ab;
    [SerializeField] private TextMeshProUGUI text;
    private void Start() {
        if(ab is ActivatedAbility){
            text.text = $"{ab.abilityName} [{((ActivatedAbility)ab).cost} Pips]:{ab.description}";
        }else text.text = $"{ab.abilityName}: {ab.description}";
    }
}
