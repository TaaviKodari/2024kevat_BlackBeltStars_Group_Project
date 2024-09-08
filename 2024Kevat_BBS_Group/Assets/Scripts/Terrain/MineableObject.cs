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

    public DroppedResource resourcePrefab;
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
        FindObjectOfType<AudioManager>().PlayOver("MineSound");
        if (hitsLeft <= 0)
        {
            DropLoot();
            FindObjectOfType<AudioManager>().PlayFull("MineDestroySound");
            Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("Mine");
        }
    }

    private void DropLoot()
    {
        foreach (ResourceDrop drop in drops)
        {
            var rd = Instantiate(resourcePrefab, transform.position, resourcePrefab.transform.rotation);
            rd.type = drop.type;
            rd.amount = drop.amount;
            rd.Init();
        }
    }
}
