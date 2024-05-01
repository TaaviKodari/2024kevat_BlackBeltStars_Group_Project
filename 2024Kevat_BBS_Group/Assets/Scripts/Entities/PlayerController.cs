using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{
    public MasterInput input { get; private set; }

    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private float arrowForce = 20;
    [SerializeField]
    private Transform shootPoint;
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        input = new MasterInput();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && input.Player.Attack.WasPerformedThisFrame())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Camera.main == null) return;
        var mousePosition = Mouse.current.position.ReadValue();
        var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var direction = (Vector2)(worldPosition - shootPoint.position);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
        var arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
        arrow.GetComponent<Arrow>().Owner = gameObject;
            
        var arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.AddForce(direction.normalized * arrowForce, ForceMode2D.Impulse);

        FindObjectOfType<AudioManager>().Play("PlayerShoot");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Resource"))
        {
            ResourceData data = collision.gameObject.GetComponent<ResourceData>();
            ResourceManager.Instance.AddResource(data.type, data.amount);
            Destroy(collision.gameObject);
        }
    }

    protected override void OnMove(Vector2 direction)
    {
        var moving = direction.sqrMagnitude > 0.1;
        animator.SetBool("Running", moving);
        if (Mathf.Abs(direction.x) > 0.1)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }

    protected override Vector2 GetMoveDirection()
    {
        return input.Player.Movement.ReadValue<Vector2>();
    }

    protected override void Die()
    {
        // TODO: Game over screen
    }
}
