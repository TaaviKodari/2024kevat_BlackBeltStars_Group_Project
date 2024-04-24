using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider healthSlider;

    public Gradient healthBarGradient;
    public Image healthBarFill;
    public void setHealth(float health)
    {
        healthSlider.value = health;

        healthBarFill.color = healthBarGradient.Evaluate(healthSlider.normalizedValue);
    }

    public void setMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;

        healthBarFill.color = healthBarGradient.Evaluate(1f);
    }
}
