using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public float timeOpen;
    private Collider2D coll;
    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            coll.enabled = false;
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.5F);
            Invoke(nameof(CloseGate), timeOpen);
        }
    }

    void CloseGate()
    {
        coll.enabled = true;
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1);
    }
}
