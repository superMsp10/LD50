using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Vector3Int worldSize;
    [SerializeField] GameObject[] blockModels;
    Transform blocksTerrain;

    enum Block
    {
        Air,
        Wood
    }

    Block[,,] blocks;

    private void Awake()
    {
        blocks = new Block[worldSize.x, worldSize.y, worldSize.z];
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < worldSize.x; x++)
            for (int y = 0; y < worldSize.y; y++)
                for (int z = 0; z < worldSize.z; z++)
                {
                    blocks[x, y, z] = (Block)Random.Range(0, blockModels.Length);
                }
        RenderWorld();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RenderWorld()
    {
        Destroy(blocksTerrain);
        blocksTerrain = new GameObject("blocksTerrain").transform;

        for (int x = 0; x < worldSize.x; x++)
            for (int y = 0; y < worldSize.y; y++)
                for (int z = 0; z < worldSize.z; z++)
                {
                    Block block = blocks[x, y, z];
                    if (block == Block.Air) continue;
                    Instantiate(blockModels[(int)block], new Vector3Int(x, y, z), Quaternion.identity, blocksTerrain);
                }
    }
}
