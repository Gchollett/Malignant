using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public HandData EnemyHand;
    public List<GameObject> Deck;
    public int startingPips = 0;
    void Awake()
    {
        if(!Instance) Instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
        
}
