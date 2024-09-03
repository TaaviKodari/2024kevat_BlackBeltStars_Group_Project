using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    
    // UI Slider component used to represent the player's health as a value between a minimum and maximum
    public Slider healthSlider;
    
    // allows the health bar color to change dynamically based on the health level, using a gradient
    public Gradient healthBarGradient;
    
    // references the color fill area of the slider
    public Image healthBarFill;
    
    // called to update the health bar when the player's health changes
    public void SetHealth(float health)
    {
        // Sets the current value of the health slider to the given health value
        healthSlider.value = health;
        
        // Changes the color of the health bar's fill area
        healthBarFill.color = healthBarGradient.Evaluate(healthSlider.normalizedValue);
        // Evaluate method takes a value between 0 and 1 and returns the corresponding color from the gradient
    }
    
    // called when the player's maximum health is set or adjusted
    public void SetMaxHealth(float health)
    {
        // Sets the max health value and changes the health to the max health value
        healthSlider.maxValue = health;
        healthSlider.value = health;
        
        // Sets the health bar's fill color to the color corresponding to full health (1.0) on the gradient
        healthBarFill.color = healthBarGradient.Evaluate(1f); 
    }
}
