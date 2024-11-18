using System;
using System.Linq;
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
    [SerializeField]
    private float fadeTime;

    private SpriteRenderer[] renderers;
    private float[] defaultAlphas;
    private float alphaMultiplier = 1;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        defaultAlphas = renderers.Select(it => it.color.a).ToArray();
    }

    private void OnValidate()
    {
        UpdateBar();
    }

    private void OnEnable()
    {
        if (value >= 1) alphaMultiplier = 0;
        UpdateBar();
    }

    private void Update()
    {
        if (!Application.isPlaying) alphaMultiplier = 1;

        if (value < 1)
        {
            alphaMultiplier = 1;
        }
        else if (alphaMultiplier > 0)
        {
            alphaMultiplier -= Time.deltaTime / fadeTime;
            if (alphaMultiplier < 0) alphaMultiplier = 0;
        }
        UpdateBar();
    }

    public void SetHealth(float health, float maxHealth)
    {
        value = health / maxHealth;
    }

    private void UpdateBar()
    {
        if (autoHide && renderers != null)
        {
            for (var i = 0; i < renderers.Length; i++)
            {
                var spriteRenderer = renderers[i];
                var alpha = alphaMultiplier * defaultAlphas[i];
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            }
        }
        barContainer.localScale = new Vector3(value, barContainer.localScale.y, barContainer.localScale.z);
        bar.color = gradient.Evaluate(value);
        bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, bar.color.a * alphaMultiplier);
    }
}
