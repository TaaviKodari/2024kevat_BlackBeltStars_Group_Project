using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MineableObject : MonoBehaviour, IBuildingBlocker
{
    private Animator animator;
    public List<ResourceDrop> drops;
    [SerializeField, CanBeNull]
    private HealthBar healthBar;
    [SerializeField, CanBeNull]
    private GameObject particlePrefab;


    [Serializable]
    public class ResourceDrop
    {
        public ResourceType type;
        public int amount;
    }
    
    public int hitsNeeded;
    private int hitsLeft;

    void Start()
    {
        hitsLeft = hitsNeeded;
        animator = GetComponent<Animator>();
        if (healthBar)
        {
            healthBar.SetHealth(hitsNeeded, hitsNeeded);
        }
    }

    public void Mine()
    {
        hitsLeft -= 1;
        AudioManager.Instance.PlayOver("MineSound");
        if (hitsLeft <= 0)
        {
            DropLoot();
            if (particlePrefab != null)
            {
                Instantiate(particlePrefab);
            }
            AudioManager.Instance.PlayFull("MineDestroySound");
            Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("Mine");
        }
        
        if (healthBar)
        {
            healthBar.SetHealth(hitsLeft, hitsNeeded);
        }
    }

    private void DropLoot()
    {
        foreach (var drop in drops)
        {
            ResourceManager.Instance.SpawnResource(drop.type, drop.amount, transform.position);
        }
    }
}
