using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = FindObjectOfType<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Temple t = FindObjectOfType<Temple>();
        if (t != null)
            navMeshAgent.SetDestination(t.transform.position);

        Debug.LogFormat("{0}", navMeshAgent.hasPath);
    }
}
