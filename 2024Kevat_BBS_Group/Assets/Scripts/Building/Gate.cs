using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public float timeOpen;
    private Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            coll.enabled = false;
            Invoke(nameof(CloseGate), timeOpen);
        }
    }

    void CloseGate()
    {
        coll.enabled = true;
    }
}
