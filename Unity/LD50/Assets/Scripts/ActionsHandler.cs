﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldManager;

public class ActionsHandler : MonoBehaviour
{
    [SerializeField] GameObject highlightBlock;
    [SerializeField] Material selected, error, destroy;

    Camera cam;
    WorldManager wManager;
    Renderer highlightBlockRenderer;

    Transform highlightBlockTransform;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        wManager = FindObjectOfType<WorldManager>();
        highlightBlockRenderer = highlightBlock.GetComponent<Renderer>();

        highlightBlockTransform = new GameObject("highlightBlockTransform").transform;
    }

    bool validBlock;
    Vector3Int mouseBlockPos;

    // Update is called once per frame
    void Update()
    {
        validBlock = GetBlockPosUnderMouse(out mouseBlockPos) && wManager.IsValidBlockPos(mouseBlockPos);
        highlightBlockRenderer.material = selected;

        if (!leftClickDrag)
        {
            highlightBlockRenderer.material = destroy;
        }
        if (!validBlock)
        {
            mouseBlockPos = wManager.GetValidWorldBlockPos(mouseBlockPos);
            highlightBlockRenderer.material = error;
        }

        highlightBlock.transform.position = mouseBlockPos;

        UpdateBuilding();
    }

    bool startDragging = false;
    bool leftClickDrag = true;
    Vector3Int startDragBlock, endDragBlock;
    void UpdateBuilding()
    {
        if (validBlock && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            startDragBlock = mouseBlockPos;
            Debug.Log("Start drag: " + startDragBlock);
            startDragging = true;
            leftClickDrag = Input.GetMouseButton(0);
        }

        //Highlight seletected blocks
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (highlightBlockTransform != null)
                Destroy(highlightBlockTransform.gameObject);
            highlightBlockTransform = new GameObject("highlightBlockTransform").transform;
            RasterLineCallback(startDragBlock, mouseBlockPos, (Vector3Int rasterPos) =>
            {
                Instantiate(highlightBlock, rasterPos, Quaternion.identity, highlightBlockTransform);
            });
        }

        if (startDragging && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
        {
            startDragging = false;
            endDragBlock = mouseBlockPos;
            Debug.Log("End drag: " + endDragBlock);

            Block b = leftClickDrag ? WorldManager.Block.Wood : WorldManager.Block.Air;
            RasterLineCallback(startDragBlock, endDragBlock, (Vector3Int rasterPos) =>
            {
                wManager.EditBlock(rasterPos, b);
            });
            wManager.RenderWorld();

            if (highlightBlockTransform != null)
                Destroy(highlightBlockTransform.gameObject);
        }
    }

    public delegate void RasterCallback(Vector3Int RasterizedBlockPos);

    //https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    void RasterLineCallback(Vector3Int p0, Vector3Int p1, RasterCallback callback)
    {
        Vector3Int dv = p1 - p0;
        if (Mathf.Abs(dv.z) < Mathf.Abs(dv.x))
        {
            if (p0.x > p1.x)
                RasterLineLowCallback(p1, p0, callback);
            else
                RasterLineLowCallback(p0, p1, callback);
        }
        else
        {
            if (p0.z > p1.z)
                RasterLineHighCallback(p1, p0, callback);
            else
                RasterLineHighCallback(p0, p1, callback);
        }
    }

    void RasterLineLowCallback(Vector3Int p0, Vector3Int p1, RasterCallback callback)
    {
        //Ignore y
        Vector3Int dv = p1 - p0;
        int yi = 1;

        if( dv.z < 0)
        {
            yi = -1;
            dv.z = -dv.z;
        }
        int d = 2 * dv.z - dv.x;
        int y = p0.z;

        for (int x = p0.x; x <= p1.x; x++)
        {

            Debug.LogFormat("Raster point low {0}", new Vector3Int(x, p0.y, y));
            callback(new Vector3Int(x, p0.y, y));
            if (d > 0)
            {
                y = y + yi;
                d = d + 2 * (dv.z - dv.x);
            }
            else
            {
                d = d + 2 * dv.z;
            }
        }
    }

    void RasterLineHighCallback(Vector3Int p0, Vector3Int p1, RasterCallback callback)
    {
        //Ignore y
        Vector3Int dv = p1 - p0;
        int xi = 1;

        if (dv.x < 0)
        {
            xi = -1;
            dv.x = -dv.x;
        }
        int d = (2 * dv.x) - dv.z;
        int x = p0.x;

        for (int y = p0.z; y <= p1.z; y++)
        {
            Debug.LogFormat("Raster point high {0}", new Vector3Int(x, p0.y, y));
            callback(new Vector3Int(x, p0.y, y));
            if (d > 0)
            {
                x = x + xi;
                d = d + 2 * (dv.x - dv.z);
            }
            else
            {
                d = d + 2 * dv.x;
            }
        }
    }

    bool GetBlockPosUnderMouse(out Vector3Int pos)
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(cam.ScreenPointToRay(Input.mousePosition));
        if (hits.Length > 0)
        {
            pos = Vector3Int.FloorToInt(hits[0].point) + new Vector3Int(1, 0, 0);
            //Debug.LogFormat("Mouse Pos {0} Block Pos {1}", hits[0].point, pos);
            return true;
        }
        pos = Vector3Int.zero;
        return false;
    }
}