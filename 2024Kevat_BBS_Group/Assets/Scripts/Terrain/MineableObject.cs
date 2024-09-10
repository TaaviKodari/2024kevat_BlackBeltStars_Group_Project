using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableObject : MonoBehaviour, IBuildingBlocker
{
    private Animator animator;
    public List<ResourceDrop> drops;

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
    }

    public void Mine()
    {
        hitsLeft -= 1;
        AudioManager.Instance.PlayOver("MineSound");
        if (hitsLeft <= 0)
        {
            DropLoot();
            AudioManager.Instance.PlayFull("MineDestroySound");
            Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("Mine");
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
