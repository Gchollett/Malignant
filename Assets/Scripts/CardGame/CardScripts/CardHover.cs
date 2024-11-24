using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHover : MonoBehaviour
{
    [SerializeField] private CreatureCard creature;
    public float scale = 1.5f;
    private void OnMouseEnter(){
        transform.localScale *= scale;
    }

    private void OnMouseExit(){
        transform.localScale = creature.initialScale;
    }
}
