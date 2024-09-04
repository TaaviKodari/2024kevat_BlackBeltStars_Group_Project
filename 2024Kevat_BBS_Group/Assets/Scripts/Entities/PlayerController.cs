using System;
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
    private BuildingPlacer buildingPlacer;
    private bool aiming;
    private float aimTime;

    protected override void Awake()
    {
        base.Awake();
        input = new MasterInput();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildingPlacer = FindObjectOfType<BuildingPlacer>();
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
        if (!EventSystem.current.IsPointerOverGameObject()
            && input.Player.Attack.WasPerformedThisFrame()
            && buildingPlacer.GetBuilding() == null)
        {
            aiming = true;
        }
        UpdateAiming();

        if (input.Player.Mine.WasPerformedThisFrame())
        {
            Mine();
        }
        if(input.Player.Movement.IsPressed())
        {
            FindObjectOfType<AudioManager>().PlayFull("WalkSound");
        }
        //not sure where else to put this lol
        FindObjectOfType<AudioManager>().PlayFull("MainTheme");
    }

    private void UpdateAiming()
    {
        if (aiming && input.Player.Attack.IsPressed())
        {
            aimTime += Time.deltaTime;
        } 
        else if (aiming && aimTime >= 1)
        {
            Shoot();
            aiming = false;
        }
        else
        {
            aimTime = 0;
            aiming = false;
        }
        animator.SetBool("Aiming", aiming);
    }

    private void Shoot()
    {
        var direction = GetAimDirection();
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        var arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
        arrow.GetComponent<Arrow>().Owner = gameObject;

        var arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.AddForce(direction.normalized * arrowForce, ForceMode2D.Impulse);

        FindObjectOfType<AudioManager>().PlayStop("PlayerShoot");
    }

    private void Mine()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<MineableObject>(out var mineable))
            {
                mineable.Mine();
                break;
            }
        }
    }

    private void UpdateLookDirection(Vector2 movementDirection)
    {
        var direction = aiming ? GetAimDirection() : movementDirection;
        if (Mathf.Abs(direction.x) > 0.1)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }

    protected override void OnMove(Vector2 direction)
    {
        var moving = direction.sqrMagnitude > 0.1;
        animator.SetBool("Running", moving);
        UpdateLookDirection(direction);
    }

    private Vector2 GetAimDirection()
    {
        if (Camera.main == null) return Vector2.zero;
        var mousePosition = Mouse.current.position.ReadValue();
        var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return worldPosition - shootPoint.position;
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
