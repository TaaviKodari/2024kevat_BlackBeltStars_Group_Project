using System;
using AtomicConsole;
using GameState;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{
    // Prefab for the arrow that the player will shoot
    [SerializeField]
    private Arrow arrowPrefab;
    // Force applied to the arrow when it's shot
    [SerializeField]
    private float arrowForce = 20;
    // Position from which the arrow will be shot
    [SerializeField]
    private Transform shootPoint;

    // References to animator, sprite renderer, and building placer
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BuildingPlacer buildingPlacer;
    private LineRenderer aimLine;
    private InGameManager gameManager;

    // State variables to track whether the player is aiming and how long they've been aiming
    private bool aiming;
    private float aimTime;

    // Called when the script instance is being loaded
    protected override void Awake()
    {
        // Call the base class's Awake method (Entity)
        base.Awake();
        // Get references to necessary components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildingPlacer = FindObjectOfType<BuildingPlacer>();
        aimLine = GetComponent<LineRenderer>();
        gameManager = FindObjectOfType<InGameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the player is trying to attack and is not interacting with the UI or building
        if (!EventSystem.current.IsPointerOverGameObject()
            && gameManager.Input.Player.Attack.WasPerformedThisFrame()
            && buildingPlacer.GetBuilding() == null)
        {
            aiming = true; // Start aiming
        }
        // Update the aiming state
        UpdateAiming();

        // Check if the player is trying to mine
        if (gameManager.Input.Player.Mine.WasPerformedThisFrame())
        {
            Mine();
        }

        // Check if the player is moving to play the walking sound
        if(gameManager.Input.Player.Movement.IsPressed())
        {
            AudioManager.Instance.PlayFull("WalkSound");
        }
    }

    // Update the aiming state based on player gameManager.Input
    private void UpdateAiming()
    {
        if (aiming && gameManager.Input.Player.Attack.IsPressed())
        {
            // Increase aim time while the attack button is pressed
            aimTime += Time.deltaTime;
        } 
        else if (aiming && aimTime >= 1)
        {
            // Shoot the arrow if aiming time exceeds 1 second
            Shoot();
            aiming = false;
        }
        else
        {
            // Reset aim time if no longer aiming
            aimTime = 0;
            aiming = false;
        }

        // Update the animator to reflect the aiming state
        animator.SetBool("Aiming", aiming);

        // Update the line
        if (aiming)
        {
            var shootPointPos = shootPoint.localPosition;
            var dir = GetAimDirection();
            aimLine.SetPositions(new Vector3[] { shootPointPos, dir * 100 - (Vector2)transform.position });
        }
    }

    // Handle shooting an arrow
    private void Shoot()
    {
        // Get the direction to aim based on the mouse position
        var direction = GetAimDirection();
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate the arrow prefab at the shoot point with the correct rotation
        var arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
        arrow.Owner = gameObject;
        arrow.Damage = damage;

        // Apply force to the arrow to shoot it
        var arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.AddForce(direction.normalized * arrowForce, ForceMode2D.Impulse);

        // Play the shooting sound effect
        AudioManager.Instance.PlayStop("PlayerShoot");
    }

    // Handle mining objects within range
    private void Mine()
    {
        // Detect nearby objects within a circle of radius 1
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (var collider in colliders)
        {
            // If the object is mineable, call its Mine method
            if (collider.TryGetComponent<MineableObject>(out var mineable))
            {
                mineable.Mine();
                break; // Stop after mining the first mineable object
            }
        }
    }

    // Update the player's look direction based on movement or aiming
    private void UpdateLookDirection(Vector2 movementDirection)
    {
        // If aiming, use the aim direction; otherwise, use the movement direction
        var direction = aiming ? GetAimDirection() : movementDirection;
        if (Mathf.Abs(direction.x) > 0.1)
        {
            // Flip the sprite if moving left, otherwise keep it facing right
            spriteRenderer.flipX = direction.x < 0;
        }
    }

    // Override the OnMove method to handle player movement and animation
    protected override void OnMove(Vector2 direction)
    {
        var moving = direction.sqrMagnitude > 0.1;
        // Update the animator to show running if the player is moving
        animator.SetBool("Running", moving);
        // Update the player's look direction based on movement
        UpdateLookDirection(direction);
    }

    // Get the direction to aim based on the mouse position
    private Vector2 GetAimDirection()
    {
        Vector3 worldPosition = GetMousePosition();
        // Return the direction from the shoot point to the mouse position
        return worldPosition - shootPoint.position;
    }

    private static Vector3 GetMousePosition()
    {
        if (Camera.main == null) return Vector2.zero;
        // Get the mouse position in world space
        var mousePosition = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // Override GetMoveDirection to return movement gameManager.Input from the player
    protected override Vector2 GetMoveDirection()
    {
        return gameManager.Input.Player.Movement.ReadValue<Vector2>();
    }

    // Override the Die method to handle player death (to be implemented)
    protected override void Die()
    {
        // TODO: Implement game over screen or death logic
    }

    [AtomicCommand("Player", "Heal", "Heal the player to max health")]
    public void HealCommandCallback()
    {
        Heal(float.PositiveInfinity);
    }
}
