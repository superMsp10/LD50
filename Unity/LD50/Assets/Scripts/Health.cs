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
        healthValue -= damage;
        if (onDamage != null)
            onDamage(this);
        if (healthValue < 0 && onDie != null)
        {
            Debug.LogFormat("{0}", "Hello!");
            onDie(this);
        }
    }
}
