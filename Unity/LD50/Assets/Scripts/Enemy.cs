using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    WorldManager worldManager;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        worldManager = FindObjectOfType<WorldManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Temple t = FindObjectOfType<Temple>();
        if (t != null)
        {
            if ((t.transform.position - transform.position).sqrMagnitude < 2)
            {
                Debug.LogFormat("{0}", "Found temple");
            }
            else
            {
                GetToTemple(t);
            }
        }

    }

    int hasValidPathScore;

    void GetToTemple(Temple t)
    {
        if (!navMeshAgent.hasPath && hasValidPathScore > -100)
        {
            hasValidPathScore--;
        }
        if (navMeshAgent.hasPath && hasValidPathScore < 100)
            hasValidPathScore++;

        //Debug.LogFormat("hasValidPathSmooth:{0}", hasValidPathScore);


        navMeshAgent.SetDestination(t.transform.position);
        if (hasValidPathScore < 0)
        {
            Vector3 templeDir = (t.transform.position - transform.position).normalized;
            Vector3Int obstacleBlock = Vector3Int.RoundToInt(transform.position + templeDir + Random.insideUnitSphere);
            obstacleBlock.y = 0;

            //FindObjectOfType<ActionsHandler>().GetHighLightBlock(obstacleBlock);
            bool isValid = worldManager.IsValidBlockPos(obstacleBlock);

            //Debug.LogFormat("isValid {0}  isObstacle {1}", isValid, isValid ? worldManager.isBlockObstacle(obstacleBlock) : false);
            if (isValid && worldManager.isBlockObstacle(obstacleBlock))
            {
                worldManager.EditBlock(obstacleBlock, WorldManager.Block.Air);
                worldManager.RenderWorld();
            }
            transform.position += templeDir * Time.smoothDeltaTime * navMeshAgent.speed;
        }

    }
}
