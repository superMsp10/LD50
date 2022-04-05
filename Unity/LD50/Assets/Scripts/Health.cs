using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float startHealth;

    public delegate void HealthCallback(Health h);
    public HealthCallback onDie, onDamage;

    public float healthValue { get; private set; }

    private void Start()
    {
        healthValue = startHealth;
    }

    public void TakeDamage(float damage)
    {

        //Debug.LogFormat("damage: {0} health: {1}", damage, healthValue);
        healthValue -= damage;
        if (onDamage != null)
            onDamage(this);
        if (healthValue <= 0 && onDie != null)
        {
            onDie(this);
        }
    }

    public void ResetHealth()
    {
        healthValue = startHealth;
        onDamage(this);
    }
}
