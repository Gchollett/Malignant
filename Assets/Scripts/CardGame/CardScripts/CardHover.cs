using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHover : MonoBehaviour
{
    [SerializeField] private readonly CreatureCard creature;
    [SerializeField] private readonly SpriteRenderer imageRenderer;
    [SerializeField] private readonly Canvas canvas;
    [SerializeField] private readonly float scale = 1.5f;
    [SerializeField] private readonly int handOffset = 1;
    [SerializeField] private readonly float speed = 3;
    [SerializeField] private readonly int hoverSortingOrder = 4;
    private int initialSortingOrder;
    private Vector2 targetPos;
    private SpriteRenderer cardRenderer;
    private Vector2 initialPos;

    void Start()
    {
        cardRenderer = transform.GetComponent<SpriteRenderer>();
        initialSortingOrder = cardRenderer.sortingOrder;
        initialPos = transform.position;
        targetPos = initialPos;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position,targetPos,speed*Time.deltaTime);
    }

    private void OnMouseEnter(){
        transform.localScale *= scale;
        cardRenderer.sortingOrder = hoverSortingOrder;
        imageRenderer.sortingOrder = hoverSortingOrder+1;
        canvas.sortingOrder = hoverSortingOrder;
        if(creature.status == CardStatus.Unplayed){
            targetPos = initialPos + new Vector2(0,handOffset);
        }
    }

    private void OnMouseExit(){
        transform.localScale = creature.initialScale;
        cardRenderer.sortingOrder = initialSortingOrder;
        imageRenderer.sortingOrder = initialSortingOrder+1;
        canvas.sortingOrder = initialSortingOrder;
        if(creature.status == CardStatus.Unplayed){
            targetPos = initialPos;
        }
    }
}
