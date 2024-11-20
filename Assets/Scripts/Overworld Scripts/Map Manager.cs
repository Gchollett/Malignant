using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public List<MapNode> nodes;
    [SerializeField]
    private int nodeNum = 8;
    [SerializeField]
    private GameObject nodePrefab;

    /// <summary>
    ///  Instaniates test nodes, Generates Edges between them, and renders them with line editor.
    /// </summary>
    private void initializeMap(){

        //Initiate each node at random Pos within camera view   
        for(int x = 0; x < nodeNum; x++){
            Vector2 pos = new Vector2(Random.Range(-9,9),Random.Range(-5,5));
            nodes.Add(Instantiate(nodePrefab, pos, Quaternion.identity, gameObject.transform).GetComponent<MapNode>());
        }

        //Randomly Assign Edges between nodes and add them to edges, as well as add lines between them
        foreach(MapNode node in nodes){
            int edgeNum = Random.Range(1,3);
            for(int x = 0; x < edgeNum; x++){

                int pickANode = Random.Range(0, nodeNum - 1);
                node.edges.Add(nodes[pickANode]);
                nodes[pickANode].edges.Add(node);
                node.GetComponent<LineRenderer>().SetPosition(0, node.transform.position);
                node.GetComponent<LineRenderer>().SetPosition(1, nodes[pickANode].transform.position);

            }
        }

    }

    private void Start() {
        initializeMap();
    }
}
