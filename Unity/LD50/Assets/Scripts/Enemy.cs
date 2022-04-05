using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float attackRadius = 2, attackDamage = 1, destructionModeSpeed = 20, attackCoolDown = 10f;

    NavMeshAgent navMeshAgent;
    WorldManager worldManager;
    Health myHealth;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        worldManager = FindObjectOfType<WorldManager>();
        myHealth = GetComponent<Health>();
        myHealth.onDie += OnDeath;
        myHealth.onDamage += OnDamage;
    }

    // Update is called once per frame
    void Update()
    {
        Temple t = FindObjectOfType<Temple>();
        if (t != null)
        {
            if ((t.transform.position - transform.position).sqrMagnitude < attackRadius)
            {
                //Debug.LogFormat("{0}", "Found temple");
                t.GetComponent<Health>().TakeDamage(attackDamage);
                myHealth.TakeDamage(1000);
            }
            else
            {
                GetToTemple(t);
            }
        }

    }

    float lastAttack;
    void GetToTemple(Temple t)
    {
        NavMeshPath p = new NavMeshPath();
        bool hasPath = navMeshAgent.CalculatePath(t.transform.position, p);
        if (p.status != NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.isStopped = true;
            Vector3 templeDir = (t.transform.position - transform.position).normalized;
            Vector3Int obstacleBlock = Vector3Int.RoundToInt(transform.position + templeDir + Random.insideUnitSphere);
            obstacleBlock.y = 0;

            //FindObjectOfType<ActionsHandler>().GetHighLightBlock(obstacleBlock);
            bool isValid = worldManager.IsValidBlockPos(obstacleBlock);

            if (isValid && worldManager.isBlockObstacle(obstacleBlock) && Time.time - lastAttack > attackCoolDown)
            {
                worldManager.EditBlock(obstacleBlock, WorldManager.Block.Air);
                lastAttack = Time.time;

                Debug.LogFormat("Enemy Attack! {0}", name);
            }

            transform.position += templeDir * Time.smoothDeltaTime * destructionModeSpeed;
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetPath(p);
        }
    }

    void OnDamage(Health h)
    {
        GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.white, h.healthValue / h.startHealth);
    }

    void OnDeath(Health h)
    {
        Destroy(gameObject);
    }
}
