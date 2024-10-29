using UnityEngine;

public class Gate : MonoBehaviour, IConnectable
{
    private static readonly int Open = Animator.StringToHash("Open");
    
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            animator.SetBool(Open, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            animator.SetBool(Open, false);
        }
    }
}
