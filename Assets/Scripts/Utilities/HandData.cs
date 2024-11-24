using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandData", menuName = "ScriptableObjects/HandData", order = 1)]
public class HandData : ScriptableObject
{
    public string handName;
    public Difficulty difficulty;
    public List<GameObject> Hand;
}
