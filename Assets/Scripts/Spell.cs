using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public static CardGameManager gm;
    public abstract void Cast();

    void Start()
    {
        gm = CardGameManager.Instance;
    }
}