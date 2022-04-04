using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] GameObject healthUI, healthUIOutline;
    float startHealthRectWidth;

    Rectangle healthInnerRect, outlineHealthRect;

    // Start is called before the first frame update
    void Start()
    {
        healthInnerRect = healthUI.GetComponent<Rectangle>();
        outlineHealthRect = healthUIOutline.GetComponent<Rectangle>();
        startHealthRectWidth = healthInnerRect.Width;
        GetComponentInParent<Health>().onDamage += OnHealthUpdate;
    }

    void OnHealthUpdate(Health h)
    {
        float percentage = h.healthValue / h.startHealth;
        healthInnerRect.Width = startHealthRectWidth * percentage;
        //outlineHealthRect.Width = startHealthRectWidth * percentage + 0.3f;

    }
}
