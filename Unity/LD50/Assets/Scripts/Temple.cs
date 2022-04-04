using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().onDie += OnDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDeath(Health h)
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
