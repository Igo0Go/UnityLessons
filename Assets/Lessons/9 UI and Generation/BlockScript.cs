using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    earth,
    grass,
    water,
    stone,
    sand,
    wood,
    wood2,
    bricks
}

public class BlockScript : MonoBehaviour {

    public BlockType type;
}
