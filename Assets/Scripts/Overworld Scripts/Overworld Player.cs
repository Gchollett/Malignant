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
    public MapNode nextNode;
    private Vector3 newLoc;
    private Vector3 startPos;

    public float movementTime = 3;
    private float elapsedTime;
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
        if(SceneManager.GetActiveScene().name == "Overworld"){
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public bool ReadyToMove(MapNode newNode) {
        if(!isMoving && newNode.edges.Contains(currentNode) || currentNode.edges.Contains(newNode)){
            return true;
        }
        return false;
    }
    public void Move(MapNode tgt){
        if(ReadyToMove(tgt)){
            currentNode = tgt;
            newLoc = tgt.transform.position;
            isMoving = true;
            nextNode = tgt;
        }else{
            Debug.Log($"Unable to move from {currentNode} to {tgt}");
        }

    }
}
