using System;
using UnityEngine;

// Manages a health bar for something
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer bar;
    [SerializeField]
    private Transform barContainer;
    [SerializeField]
    private Gradient gradient;

    [Range(0, 1)]
    [SerializeField]
    private float value;
    [SerializeField]
    private bool autoHide = true;

    private SpriteRenderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnValidate()
    {
        UpdateBar();
    }

    private void OnEnable()
    {
        UpdateBar();
    }

    public void SetHealth(float health, float maxHealth)
    {
        value = health / maxHealth;
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (autoHide && renderers != null)
        {
            foreach (var spriteRenderer in renderers)
            {
                spriteRenderer.enabled = value < 1;
            }
        }
        barContainer.localScale = new Vector3(value, barContainer.localScale.y, barContainer.localScale.z);
        bar.color = gradient.Evaluate(value);
    }
}
