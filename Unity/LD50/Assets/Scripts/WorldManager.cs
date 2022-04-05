using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Vector3Int worldSize;
    [SerializeField] GameObject[] blockModels;
    public BlockProperties[] blockProperties;

    [SerializeField] float gateSize = 1;

    Dictionary<Vector3Int, GameObject> blockToModelInstance;

    Transform blocksTerrain;

    WaveManager waveM;


    [System.Serializable]
    public struct BlockProperties
    {
        public bool isEditable;
        public bool isObstacle;
        public int cost;
    }

    public enum Block
    {
        Air,
        Wall,
        Temple,
        Wood,
        Spike
    }

    public Block[,,] blocks;

    private void Awake()
    {
        blocks = new Block[worldSize.x, worldSize.y, worldSize.z];
        blockToModelInstance = new Dictionary<Vector3Int, GameObject>();
        //Build edge walls and gates
        for (int x = 0; x < worldSize.x; x++)
            for (int z = 0; z < worldSize.z; z++)
            {
                //Gates in the middle
                if (x == 0 || z == 0 || x == (worldSize.x - 1) || z == (worldSize.z - 1))
                {
                    float distXCenter = Mathf.Abs(x - worldSize.x / 2);
                    float distZCenter = Mathf.Abs(z - worldSize.z / 2);

                    if (distXCenter > gateSize && distZCenter > gateSize)
                        blocks[x, 0, z] = Block.Wall;
                }
            }

        //Place temple
        blocks[worldSize.x / 2, 0, worldSize.z / 2] = Block.Temple;
        RenderWorld();
    }

    // Start is called before the first frame update
    void Start()
    {
        waveM = FindObjectOfType<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool isBlockObstacle(Vector3Int pos)
    {
        //Debug.LogFormat("isBlockObstacle Pos:{2} Block{0} Obstacle:{1}", blocks[pos.x, pos.y, pos.z], blockProperties[(int)blocks[pos.x, pos.y, pos.z]].isObstacle, pos);
        return blockProperties[(int)blocks[pos.x, pos.y, pos.z]].isObstacle;
    }

    public bool isBlockEditable(Vector3Int pos)
    {
        return blockProperties[(int)blocks[pos.x, pos.y, pos.z]].isEditable;
    }

    public void EditBlock(Vector3Int pos, Block b)
    {
        if (isBlockEditable(pos))
        {
            //Debug.LogFormat("money {0} cost{1} block{2}", waveM.money, blockProperties[((int)b)].cost, b);
            waveM.money += blockProperties[((int)blocks[pos.x, pos.y, pos.z])].cost;
            if (waveM.money >= blockProperties[((int)b)].cost)
            {
                blocks[pos.x, pos.y, pos.z] = b;
                waveM.money -= blockProperties[((int)b)].cost;
                if (blockToModelInstance.ContainsKey(pos) && blockToModelInstance[pos] != null)
                    Destroy(blockToModelInstance[pos]);
                if (blockModels[(int)b] != null)
                    blockToModelInstance[pos] = Instantiate(blockModels[(int)b], pos, Quaternion.identity, blocksTerrain);
            }
        }
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
                    if (blockModels[(int)block] == null) continue;
                    Vector3Int pos = new Vector3Int(x, y, z);
                    blockToModelInstance[pos] = Instantiate(blockModels[(int)block], pos, Quaternion.identity, blocksTerrain);
                }
    }
}
