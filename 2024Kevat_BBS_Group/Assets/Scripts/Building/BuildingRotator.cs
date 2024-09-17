using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRotator : MonoBehaviour, IConnectable
{
    public Sprite horizontal;
    public Sprite vertical;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = EnemyManager.instance.player;
        Vector3 distance = transform.position - player.transform.position;
        float xDiff = Mathf.Abs(distance.x);
        float yDiff = Mathf.Abs(distance.y);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (xDiff > yDiff) sr.sprite = vertical;
        else sr.sprite = horizontal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
