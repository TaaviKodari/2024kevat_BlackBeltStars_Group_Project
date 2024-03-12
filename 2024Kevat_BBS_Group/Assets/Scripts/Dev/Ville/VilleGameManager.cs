using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VilleGameManager : MonoBehaviour
{
    public float playerHealth;
    public TextMeshProUGUI healthText;
    
    public GameObject playerGameObject;
    
    void Awake()
    {
        playerHealth = 100.0f;
    }

    void Start()
    {
        
    }

    void Update()
    {
        healthText.text = "Health: "+playerHealth.ToString();     
        if(playerHealth < 1){
            playerDeath();
        }  
    }

    public void playerDeath(){
        Debug.Log("You died!");
    }
}
