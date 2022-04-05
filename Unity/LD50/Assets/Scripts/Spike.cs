using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] GameObject spikes;
    [SerializeField] float attackCoolDown;

    float lastAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastAttack < attackCoolDown)
        {
            GetComponent<Collider>().enabled = false;
            spikes.SetActive(false);
        }
        else
        {
            GetComponent<Collider>().enabled = true;
            spikes.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Health>().TakeDamage(0.5f);
        lastAttack = Time.time;
    }
}
