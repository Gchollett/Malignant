using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
     [SerializeField]
    public List<List<MapNode>> nodes = new List<List<MapNode>>();
    [SerializeField]
    private int depth;
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private GameObject player;

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


    /// <summary>
    ///  Instaniates test nodes, Generates Edges between them, and renders them with line editor.
    ///  Very temporary and always changing.
    /// </summary>
    private void initializeMap(){

        int xPos = -8;
        int yPos = -4;
        int nodeNum = 1;   
        for(int x = 0; x < depth; x++){
            List<MapNode> newLst = new List<MapNode>();
            nodes.Add(newLst);
            for(int y = 0; y < nodeNum; y++){
                if(nodeNum==1)yPos += 8/2; else yPos += 8/nodeNum;
                //Debug.Log($"Node_{x},{y} yPos: {yPos}");
                Vector2 pos = new Vector2(xPos,yPos);
                GameObject newNode = Instantiate(nodePrefab, pos, Quaternion.identity, gameObject.transform);
                nodes[x].Add(newNode.GetComponent<TestNode>());
                newNode.name = $"Node_{x},{y}";
            }
            nodeNum *= 2;
            yPos = -4;
            xPos += 16/depth; //NEEDS TO CHANGE
        }

        //Randomly Assign Edges between nodes and add them to edges, as well as add lines between them
        //Currently adds Edges only to nodes in the next List stored in nodes
        for(int x = 0; x < depth -1; x++){
                foreach (MapNode node in nodes[x]){
                    //Debug.Log($"On {node.name}");
                    int edgeNum = Random.Range(1,3);
                    for(int i = 0; i < edgeNum; i++){
                        int pickANode = Random.Range(0, nodes[x+1].Count - 1);
                        Debug.Log(pickANode);
                        node.edges.Add(nodes[x +1][pickANode]);
                        nodes[x+1][pickANode].edges.Add(node);
                        node.GetComponent<LineRenderer>().SetPosition(0, node.transform.position);
                        node.GetComponent<LineRenderer>().SetPosition(1, nodes[x+1][pickANode].transform.position);
                    }
                }
        }
    }


    private void initializePlayer(){
        Debug.Log("Running");
        player.transform.position = nodes[0][0].transform.position - new Vector3(0,0,3);
        player.GetComponent<OverworldPlayer>().initializePlayer();

    }

    private void Start() {
        initializeMap();
        initializePlayer();
    }

    public void MovePlayer(Vector3 pos, MapNode tgt){
        Debug.Log(player);
        Debug.Log(nodes[0][0]);
        MapNode currentNode = player.GetComponent<OverworldPlayer>().currentNode;
        Debug.Log(currentNode);
        if(currentNode.edges.Contains(tgt)){
            player.transform.position = pos - new Vector3(0,0,3);
            player.GetComponent<OverworldPlayer>().currentNode = tgt;
        }else{
            Debug.Log($"Unable to move from {currentNode} to {tgt}");
        }

    }
}
