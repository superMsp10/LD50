using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Vector3Int worldSize;
    [SerializeField] GameObject[] blockModels;
    Transform blocksTerrain;

    public enum Block
    {
        Air,
        Wood
    }

    public Block[,,] blocks;

    private void Awake()
    {
        blocks = new Block[worldSize.x, worldSize.y, worldSize.z];
    }

    // Start is called before the first frame update
    void Start()
    {
        //for (int x = 0; x < worldSize.x; x++)
        //    for (int y = 0; y < worldSize.y; y++)
        //        for (int z = 0; z < worldSize.z; z++)
        //        {
        //            blocks[x, y, z] = (Block)Random.Range(0, blockModels.Length);
        //        }
        RenderWorld();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EditBlock(Vector3Int pos, Block b)
    {
        //if (IsValidBlockPos(pos))
        //{
        //    Debug.LogFormat("{0}", "Hello!");
            blocks[pos.x, pos.y, pos.z] = b;
        //}
    }

    public Vector3Int GetValidWorldBlockPos(Vector3Int pos)
    {
        if (pos.x >= worldSize.x) pos.x = worldSize.x - 1;
        if (pos.y >= worldSize.y) pos.y = worldSize.y - 1;
        if (pos.z >= worldSize.z) pos.z = worldSize.z - 1;

        if (pos.x < 0) pos.x = 0;
        if (pos.y < 0) pos.y = 0;
        if (pos.z < 0) pos.z = 0;

        return pos;
    }

    public bool IsValidBlockPos(Vector3Int pos)
    {
        return (pos.x < worldSize.x && pos.y < worldSize.y && pos.z < worldSize.z &&
            pos.x >= 0 && pos.y >= 0 && pos.z >= 0);
    }

    public void RenderWorld()
    {
        if (blocksTerrain != null)
            Destroy(blocksTerrain.gameObject);
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
