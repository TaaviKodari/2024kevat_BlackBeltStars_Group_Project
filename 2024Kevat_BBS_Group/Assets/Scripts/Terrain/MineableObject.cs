using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableObject : MonoBehaviour, IBuildingBlocker
{
    public List<ResourceDrop> drops;

    [Serializable]
    public class ResourceDrop
    {
        public ResourceManager.ResourceType type;
        public int amount;
    }

    public Resource resourcePrefab;
    public int hitsNeeded;
    private int hitsLeft;

    // Start is called before the first frame update
    void Start()
    {
        hitsLeft = hitsNeeded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Mine()
    {
        hitsLeft -= 1;
        if (hitsLeft <= 0)
        {
            DropLoot();
            Destroy(gameObject);
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
