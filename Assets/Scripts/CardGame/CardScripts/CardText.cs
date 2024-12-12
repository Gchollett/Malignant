using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private List<TextMeshProUGUI> abilityText;
    public List<Button> abilityButtons;
    [SerializeField] private Card creature;

    private void FixedUpdate() {
        setCardText();
    }

    private void setCardText(){
        string adjectives = "";
        foreach (StatusEffect se in creature.statusEffects.Keys){
            if(creature.statusEffects[se] > 0) adjectives = se.effectName + " " + adjectives;
        }
        foreach (StatusEffect se in creature.staticEffects){
            adjectives = se.effectName + " " + adjectives;
        }
        nameText.text = adjectives + creature.cardData.cardName;
        powerText.text = ((creature.cardData.power + creature.tempPower >= 0)?(creature.cardData.power + creature.tempPower):0).ToString();
        healthText.text = (creature.cardData.health + creature.tempHealth).ToString();
        for(int i = 0; i < creature.abilityLimit; i++){
            Ability ab = i<creature.abilities.Count?creature.abilities[i]:null;
            if(ab){
                abilityText[i].text = ab.abilityName;
                if(ab is ActivatedAbility){
                    abilityButtons[i].gameObject.SetActive(true);
                    abilityButtons[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{((ActivatedAbility)ab).cost} pips";
                }
            } else {
                abilityText[i].text = "";
            }
        }
    }
}
