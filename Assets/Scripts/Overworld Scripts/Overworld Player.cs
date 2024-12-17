using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldPlayer : MonoBehaviour
{
    public static OverworldPlayer Instance;
    MapManager mm;
    public MapNode currentNode;

    public float speed = 3;
    public bool isMoving {get; private set;} = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void initializePlayer()
    {   
        mm = MapManager.Instance;
        currentNode = mm.nodes[0].GetComponent<MapNode>();
        Debug.Log($"Player started at node {currentNode}");
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (isMoving) {
            transform.position = Vector3.MoveTowards(transform.position,currentNode.transform.position,speed*Time.deltaTime);
            if (Vector3.Distance(transform.position,currentNode.transform.position) <= .001) {
                isMoving = false;
            }
        }
        if(SceneManager.GetActiveScene().name == "Overworld"){
            GetComponent<SpriteRenderer>().enabled = true;
        }else{
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public bool AbleToMove(MapNode newNode) {
        if(!isMoving && newNode.edges.Contains(currentNode) || currentNode.edges.Contains(newNode)){
            return true;
        }
        return false;
    }
    public void Move(MapNode tgt){
        if(AbleToMove(tgt)){
            currentNode = tgt;
            isMoving = true;
        }else{
            Debug.Log($"Unable to move from {currentNode} to {tgt}");
        }

    }
}
