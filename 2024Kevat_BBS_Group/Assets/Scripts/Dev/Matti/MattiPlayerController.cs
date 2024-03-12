using UnityEngine;

public class MattiPlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    private Rigidbody2D rb;
    private MasterInput controls;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new MasterInput();
    }
    
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var movementDirection = controls.Player.Movement.ReadValue<Vector2>();
        rb.MovePosition(rb.position + movementDirection * (movementSpeed * Time.fixedDeltaTime));
    }
}
