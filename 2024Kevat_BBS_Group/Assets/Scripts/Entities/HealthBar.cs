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

    private void OnValidate()
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
        barContainer.localScale = new Vector3(value, barContainer.localScale.y, barContainer.localScale.z);
        bar.color = gradient.Evaluate(value);
    }
}
