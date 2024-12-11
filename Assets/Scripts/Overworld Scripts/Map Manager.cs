using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public int nodeNum;
    [SerializeField]
    private GameObject startNodePrefab;
    [SerializeField]
    private GameObject battleNodePrefab;
    [SerializeField]
    private GameObject bossNodePrefab;
    [SerializeField]
    private GameObject abilityStoreNodePrefab;
    [SerializeField]
    private GameObject cardStoreNodePrefab;
    [SerializeField]
    private GameObject hunterNodePrefab;
    [SerializeField]
    private GameObject monolithNodePrefab;
    [SerializeField]
    private GameObject treasureNodePrefab;
    [SerializeField]
    private GameObject upgradeNodePrefab;
    [SerializeField]
    private GameObject treeNodePrefab;
    [SerializeField]
    private GameObject edgePrefab;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float radius;
    [SerializeField]
    private Vector2 sampleRegionSize;
    public int numVisited {get; set;} = 0;

    public List<GameObject> nodes {get; private set;} = new List<GameObject>();
    public List<GameObject> edges {get; private set;} = new List<GameObject>();

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
    private void initializeMap()
    {
        AddNodes(); 
        AddEdges();
    }

    private void AddNodes()
    {
        List<Vector2> points = new List<Vector2>();
        while(points.Count < nodeNum || Mathf.Floor(points.Count/nodeNum)*nodeNum < points.Count){
            points = PoissonDiscSampling.GeneratePoints(radius,sampleRegionSize);
        }
        points.Sort((Vector2 a, Vector2 b) => (int)Mathf.Round(a.x-b.x));
        int p_count = points.Count/nodeNum;
        for(int i=0; i < nodeNum; i++){
            if(i == 0){
                GameObject newNode = Instantiate(startNodePrefab,transform);
                newNode.transform.position = points[i*p_count] - sampleRegionSize/2;
                nodes.Add(newNode);
            } else if(i <= nodeNum/3){
                List<GameObject> nodeList = new List<GameObject>
                    {
                        battleNodePrefab,
                        battleNodePrefab,
                        battleNodePrefab,
                        battleNodePrefab,
                        treasureNodePrefab,
                        treasureNodePrefab,
                        cardStoreNodePrefab,
                    };
                GameObject pickedNode = nodeList[Random.Range(0,nodeList.Count)];
                GameObject newNode = Instantiate(pickedNode,transform);
                newNode.transform.position = points[i*p_count] - sampleRegionSize/2;
                nodes.Add(newNode);
            }else if(i <= nodeNum*3/4){
                List<GameObject> nodeList = new List<GameObject>
                    {
                        battleNodePrefab,
                        battleNodePrefab,
                        battleNodePrefab,
                        battleNodePrefab,
                        battleNodePrefab,
                        battleNodePrefab,
                        battleNodePrefab,
                        upgradeNodePrefab,
                        upgradeNodePrefab,
                        treasureNodePrefab,
                        treasureNodePrefab,
                        cardStoreNodePrefab,
                        cardStoreNodePrefab,
                        hunterNodePrefab,
                        hunterNodePrefab,
                        abilityStoreNodePrefab,
                        abilityStoreNodePrefab,
                        monolithNodePrefab
                    };
                GameObject pickedNode = nodeList[Random.Range(0,nodeList.Count)];
                GameObject newNode = Instantiate(pickedNode,transform);
                newNode.transform.position = points[i*p_count] - sampleRegionSize/2;
                nodes.Add(newNode);
            }else if(i < nodeNum-1){
                List<GameObject> nodeList = new List<GameObject>
                    {
                        battleNodePrefab,
                        abilityStoreNodePrefab,
                        monolithNodePrefab,
                        monolithNodePrefab,
                    };
                GameObject pickedNode = nodeList[Random.Range(0,nodeList.Count)];
                GameObject newNode = Instantiate(pickedNode,transform);
                newNode.transform.position = points[i*p_count] - sampleRegionSize/2;
                nodes.Add(newNode);
            }else{
                GameObject newNode = Instantiate(bossNodePrefab,transform);
                newNode.transform.position = points[i*p_count] - sampleRegionSize/2;                
                nodes.Add(newNode);
            }
        }
        for(int i=0;i<points.Count;i++){
            if(i%p_count != 0){
                GameObject newNode = Instantiate(treeNodePrefab,transform);
                newNode.transform.position = points[i] - sampleRegionSize/2; 
            }

        }
    }

    private void AddEdges()
    {
        GameObject currentNode = nodes[0];
        for(int i = 1; i < nodes.Count; i++){
            GameObject neighborNode = nodes[i];
            GameObject edge = Instantiate(edgePrefab,transform);
            edge.GetComponent<LineRenderer>().SetPosition(0,currentNode.transform.position);
            edge.GetComponent<LineRenderer>().SetPosition(1,neighborNode.transform.position);
            currentNode.GetComponent<MapNode>().edges.Add(neighborNode.GetComponent<MapNode>());
            edges.Add(edge);
            currentNode = neighborNode;
        }
        currentNode = nodes[0];
        for(int i = Random.Range(2,4); i < nodes.Count; i+=Random.Range(2,5)){
            GameObject neighborNode = nodes[i];
            GameObject edge = Instantiate(edgePrefab,transform);
            edge.GetComponent<LineRenderer>().SetPosition(0,currentNode.transform.position);
            edge.GetComponent<LineRenderer>().SetPosition(1,neighborNode.transform.position);
            currentNode.GetComponent<MapNode>().edges.Add(neighborNode.GetComponent<MapNode>());
            edges.Add(edge);
            currentNode = neighborNode;
        }
    }



    private void initializePlayer(){
        player.transform.position = nodes[0].transform.position - new Vector3(0,0,3);
        player.GetComponent<OverworldPlayer>().initializePlayer();
    }

    private void Start() {
        if(nodes.Count > 0){
            foreach(GameObject node in nodes){
                Instantiate(node);
            }
            foreach(GameObject edge in edges){
                Instantiate(edge);
            }
        }else{
            initializeMap();
            initializePlayer();
        }
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate() {
        if(SceneManager.GetActiveScene().name == "Overworld"){
            for(int i = 0; i< transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }else{
            for(int i = 0; i< transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

}
