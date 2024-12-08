using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OverworldPlayer : MonoBehaviour
{
    public static OverworldPlayer Instance;
    MapManager mm;
    public MapNode currentNode;
    public MapNode nextNode;
    private Vector3 newLoc;
    private Vector3 startPos;

    public float movementTime = 3;
    private float elapsedTime;
    private bool isMoving = false;

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

    public void initializePlayer()
    {   
        Debug.Log("Running");
        mm = MapManager.Instance;
        currentNode = mm.nodes[0][0];
        Debug.Log($"Player started at node {currentNode}");
        startPos = currentNode.transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (isMoving) {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, newLoc, elapsedTime / movementTime);
            if (elapsedTime >= movementTime) {
                transform.position = newLoc;
                elapsedTime = 0;
                isMoving = false;
                currentNode = nextNode;
                startPos = nextNode.transform.position;
            }
        }
    }

    public void NextNode(MapNode newNode) {
        nextNode = newNode;
        newLoc = nextNode.transform.position + new Vector3 (0, 0, -3);
        isMoving = true;
    }

    public bool ReadyToMove(MapNode newNode) {
        if(!isMoving && newNode.edges.Contains(currentNode) || currentNode.edges.Contains(newNode)){
            return true;
        }
        return false;
    }
}
