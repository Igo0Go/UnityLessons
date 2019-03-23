using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IgoGoArray
{
    public List<GameObject> gameObjects;
}

public class Homework1_IgoGo : MonoBehaviour {

    [Header("Домашняя работа№1")]
    [Space(30)]
    [Tooltip(":)")]
    public List<IgoGoArray> igoGos;
}
