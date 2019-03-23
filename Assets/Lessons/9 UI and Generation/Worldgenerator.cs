using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityBufer
{
    public BlockType type;
    public int value;

    public PriorityBufer(BlockType blockType)
    {
        type = blockType;
    }
}

public class Worldgenerator : MonoBehaviour {

    public GameObject player;
    public GameObject[] spawnBufer;
    public GameObject waterBlock;
    public GameObject grassBlock;
    public GameObject wood1Block;
    public GameObject wood1Block2;
    public Vector3Int size;


    private GameObject[,,] world;
    private Vector3Int currentPos;
    private List<GameObject> currentBufer;
    private GameObject spawnObject;

	// Use this for initialization
	void Start () {
        GenerateStandartBlock();
        GenerateWater();
        GenerateGrass();
        GenerateWoods();
        GenerateWood2();
        Instantiate(player, GetPlayerSpawn(), Quaternion.identity);
    }

    #region Вспомогательные
    private Vector3 GetPlayerSpawn()
    {
        return new Vector3(size.x / 2, size.y + 3, size.z / 2);
    }
    private bool GetBlock(GameObject origin, out BlockScript block)
    {
        block = null;
        if (origin != null)
        {
            block = origin.GetComponent<BlockScript>();
            if (block != null)
            {
                return true;
            }
        }
        return false;
    }
    private void AddThreeType(BlockType blockType)
    {
        GameObject block = null;
        foreach (var c in spawnBufer)
        {
            if (c.GetComponent<BlockScript>().type == blockType)
            {
                block = c;
            }
        }
        currentBufer.Add(block);
        currentBufer.Add(block);
        currentBufer.Add(block);
    }
    private void RemoveBlockType(BlockType type)
    {
        foreach (var c in currentBufer)
        {
            if (c.GetComponent<BlockScript>().type == type)
            {
                currentBufer.Remove(c);
                return;
            }
        }
    }
    private bool TypeUnder(BlockType blockType)
    {
        BlockScript block;
        if(GetBlock(world[currentPos.x, currentPos.y - 1, currentPos.z], out block))
        {
            if(block.type == blockType)
            {
                return true;
            }

        }
        return false;
    }
    private bool NearWithType(BlockType blockType)
    {
        GameObject forward, back, right, left;
        List<GameObject> neighbours = new List<GameObject>();


        if (currentPos.z < size.z - 1)
        {
            forward = world[currentPos.x, currentPos.y, currentPos.z + 1];
            neighbours.Add(forward);
        }

        if (currentPos.z > 0)
        {
            back = world[currentPos.x, currentPos.y, currentPos.z - 1];
            neighbours.Add(back);
        }
        if (currentPos.x < size.x - 1)
        {
            right = world[currentPos.x + 1, currentPos.y, currentPos.z];
            neighbours.Add(right);
        }
        if (currentPos.x > 0)
        {
            left = world[currentPos.x - 1, currentPos.y, currentPos.z];
            neighbours.Add(left);
        }


        BlockScript block;

        foreach (var c in neighbours)
        {
            if (GetBlock(c, out block))
            {
                if (block.type == blockType)
                {
                    return true;
                }
            }
        }

        return false;
    }
    private bool IsWoodUp()
    {
        GameObject[] forward, back, right, left;
        forward = new GameObject[3];
        back = new GameObject[3];
        right = new GameObject[3];
        left = new GameObject[3];
        List<GameObject[]> neighbours = new List<GameObject[]>();

        if(currentPos.y == size.y - 1)
        {
            return false;
        }

        if (currentPos.z < size.z - 1)
        {
            forward[0] = world[currentPos.x, currentPos.y, currentPos.z + 1];
            forward[1] = world[currentPos.x, currentPos.y + 1, currentPos.z + 1];
            forward[2] = world[currentPos.x, currentPos.y - 1, currentPos.z];
            neighbours.Add(forward);
        }
        if (currentPos.z > 0)
        {
            back[0] = world[currentPos.x, currentPos.y, currentPos.z - 1];
            back[1] = world[currentPos.x, currentPos.y + 1, currentPos.z - 1];
            back[2] = world[currentPos.x, currentPos.y - 1, currentPos.z];
            neighbours.Add(back);
        }
        if (currentPos.x < size.x - 1)
        {
            right[0] = world[currentPos.x + 1, currentPos.y, currentPos.z];
            right[1] = world[currentPos.x + 1, currentPos.y+1, currentPos.z];
            right[2] = world[currentPos.x, currentPos.y - 1, currentPos.z];
            neighbours.Add(right);
        }
        if (currentPos.x > 0)
        {
            left[0] = world[currentPos.x - 1, currentPos.y, currentPos.z];
            left[1] = world[currentPos.x - 1, currentPos.y + 1, currentPos.z];
            left[2] = world[currentPos.x, currentPos.y - 1, currentPos.z];
            neighbours.Add(left);
        }


        BlockScript block;

        foreach (var c in neighbours)
        {
            if (GetBlock(c[0], out block))
            {
                if (block.type == BlockType.wood)
                {
                    if(c[1] == null && c[2] == null)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    private bool IsWoodDown()
    {
        if(currentPos.y > 2)
        {
            GameObject obj = world[currentPos.x, currentPos.y - 2, currentPos.z];
            BlockScript block;
            if(GetBlock(obj, out block))
            {
                if(block.type != BlockType.wood)
                {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    #region Первичная генерация
    private void GenerateStandartBlock()
    {
        world = new GameObject[size.x, size.y, size.z];

        for (int y = 0; y < size.y/2; y++)
        {
            for (int z = 0; z < size.z; z++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    currentPos = new Vector3Int(x, y, z);
                    CreateBlueprintList();

                    if (currentBufer != null)
                    {
                        if (currentBufer.Count > 0)
                        {
                            spawnObject = currentBufer[Random.Range(0, currentBufer.Count)];

                            if (spawnObject != null)
                            {
                                world[x, y, z] = Instantiate(spawnObject, new Vector3(x, y, z), Quaternion.identity);
                            }
                        }
                    }
                }
            }
        }
    }
    private void CreateBlueprintList()
    {
        currentBufer = new List<GameObject>(spawnBufer);

        if (currentPos.y > 0)
        {
            currentBufer.Add(null);
            BlockScript block;
            if (GetBlock(world[currentPos.x, currentPos.y - 1, currentPos.z], out block))
            {
                if (block.type == BlockType.sand)
                {
                    RemoveBlockType(BlockType.earth);
                }
            }
        }
        AddPriorityBlock();
    }
    private void AddPriorityBlock()
    {
        List<PriorityBufer> priorityBlock = new List<PriorityBufer>();
        priorityBlock.Add(new PriorityBufer(BlockType.earth));
        priorityBlock.Add(new PriorityBufer(BlockType.sand));
        priorityBlock.Add(new PriorityBufer(BlockType.stone));

        GameObject forward, back, right, left;
        List<GameObject> neighbours = new List<GameObject>();

        if (currentPos.z < size.z - 1)
        {
            forward = world[currentPos.x, currentPos.y, currentPos.z + 1];
            neighbours.Add(forward);
        }

        if (currentPos.z > 0)
        {
            back = world[currentPos.x, currentPos.y, currentPos.z - 1];
            neighbours.Add(back);
        }
        if (currentPos.x < size.x - 1)
        {
            right = world[currentPos.x + 1, currentPos.y, currentPos.z];
            neighbours.Add(right);
        }
        if (currentPos.x > 0)
        {
            left = world[currentPos.x - 1, currentPos.y, currentPos.z];
            neighbours.Add(left);
        }

        foreach (var c in neighbours)
        {
            foreach (var t in priorityBlock)
            {
                BlockScript blockScript;
                if (GetBlock(c, out blockScript))
                {
                    if (t.type == blockScript.type)
                    {
                        t.value = t.value + 1;
                    }
                }
            }
        }

        BlockType maxType = BlockType.earth;
        int maxValue = priorityBlock[0].value;
        foreach (var c in priorityBlock)
        {
            if (c.value > maxValue)
            {
                maxValue = c.value;
                maxType = c.type;
            }
        }

        AddThreeType(maxType);
    }
    #endregion

    #region Вторичная генерация
    private void GenerateWater()
    {
        for (int y = 0; y < size.y/2; y++)
        {
            for (int z = 0; z < size.z; z++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    currentPos = new Vector3Int(x, y, z);
                    if(world[x, y, z] == null)
                    {
                        int maxRange = 2;
                        if(TypeUnder(BlockType.water))
                        {
                            maxRange++;
                        }
                        int key = Random.Range(0, maxRange);
                        if(key > 0)
                        {
                            world[x, y, z] = Instantiate(waterBlock, new Vector3(x, y, z), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }
    private void GenerateGrass()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int z = 0; z < size.z; z++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    currentPos = new Vector3Int(x, y, z);
                    if (world[x, y, z] == null)
                    {
                        int maxRange = 2;
                        int key = 2;
                        BlockScript block;
                        if(GetBlock(world[x,y-1,z], out block))
                        {
                            if(block.type == BlockType.earth)
                            {
                                key = 0;
                            }
                            else if(block.type == BlockType.sand)
                            {
                                maxRange = 2;
                                key = Random.Range(0, maxRange);
                            }
                            else if (block.type == BlockType.stone)
                            {
                                maxRange = 3;
                                key = Random.Range(0, maxRange);
                            }

                            if (key == 0)
                            {
                                world[x, y, z] = Instantiate(grassBlock, new Vector3(x, y, z), Quaternion.identity);
                            }
                        }
                    }
                }
            }
        }
    }
    private void GenerateWoods()
    {
        for (int y = 0; y < size.y-1; y++)
        {
            for (int z = 0; z < size.z; z++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    currentPos = new Vector3Int(x, y, z);
                    if (world[x, y, z] == null)
                    {
                        if(TypeUnder(BlockType.grass) && !NearWithType(BlockType.wood))
                        {
                            int key = Random.Range(0, size.x/2);
                            if (key < size.x/10 || key == 0)
                            {
                                world[x, y, z] = Instantiate(wood1Block, new Vector3(x, y, z), Quaternion.identity);
                            }
                        }
                        else if(TypeUnder(BlockType.wood))
                        {
                            int key = Random.Range(0, 2);
                            if (key == 0)
                            {
                                world[x, y, z] = Instantiate(wood1Block, new Vector3(x, y, z), Quaternion.identity);
                            }
                        }
                    }
                }
            }
        }
    }
    private void GenerateWood2()
    {
        for (int y = size.y/2; y < size.y; y++)
        {
            for (int z = 0; z < size.z; z++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    currentPos = new Vector3Int(x, y, z);
                    if (world[x, y, z] == null)
                    {
                        if (TypeUnder(BlockType.wood) || IsWoodUp())
                        {
                            world[x, y, z] = Instantiate(wood1Block2, new Vector3(x, y, z), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }
    #endregion

}
