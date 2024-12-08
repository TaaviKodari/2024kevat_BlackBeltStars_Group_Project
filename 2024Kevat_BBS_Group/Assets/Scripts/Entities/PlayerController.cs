using AtomicConsole;
using GameState;
using Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Attributes;
using Attribute = Attributes.Attribute;

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

    // Reference to the GameStateManager
    private GameStateManager gameStateManager;

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
        gameStateManager = FindObjectOfType<GameStateManager>();
    }

    // Called when the object becomes enabled and active
    private void OnEnable()
    {
        if (gameStateManager != null)
        {
            InitPlayerAttributes();
        }
    }

    // Initialize player attributes based on the current save game
    private void InitPlayerAttributes()
    {
        for (var i = 0; i < gameStateManager.currentSaveGame.inventory.speedBoosts.Count; i++)
        {
            var speedBoost = gameStateManager.currentSaveGame.inventory.speedBoosts[i];
            
            if(speedBoost.duration >= 1)
            {
                AttributeHolder.AddModifier(new AttributeModifier
                {
                    Tag = "speed",
                    Attribute = Attribute.Find("speed"),
                    Type = AttributeModifierType.Multiply,
                    Amount = speedBoost.multiplier
                });
                speedBoost.duration--;
                gameStateManager.currentSaveGame.inventory.speedBoosts[i] = speedBoost;
            }
            else
            {
                Debug.Log("Removing speed boost: "+speedBoost+" due to the duration running out");
                gameStateManager.currentSaveGame.inventory.speedBoosts.RemoveAll(boost => boost.duration <= 0);
            }
        }
        
        for (var i = 0; i < gameStateManager.currentSaveGame.inventory.healthBoosts.Count; i++)
        {
            var healthBoost = gameStateManager.currentSaveGame.inventory.healthBoosts[i];
            if (healthBoost.duration >= 1)
            {
                AttributeHolder.AddModifier(new AttributeModifier
                {
                    Tag = "max_health",
                    Attribute = Attribute.Find("max_health"),
                    Type = AttributeModifierType.Multiply,
                    Amount = healthBoost.multiplier
                });
                healthBoost.duration--;
                gameStateManager.currentSaveGame.inventory.healthBoosts[i] = healthBoost;

            }
            else
            {
                Debug.Log("Removing health boost: '"+healthBoost+"' due to the duration running out");
                gameStateManager.currentSaveGame.inventory.healthBoosts.RemoveAll(boost => boost.duration <= 0);
            }
            
        }

        for (var i = 0; i < gameStateManager.currentSaveGame.inventory.damageBoosts.Count; i++)
        {
            var damageBoost = gameStateManager.currentSaveGame.inventory.damageBoosts[i];
            if(damageBoost.duration >= 1)
            {
                if (damageBoost.multiplier != 0)
                {
                    AttributeHolder.AddModifier(new AttributeModifier
                    {
                        Tag = "damage",
                        Attribute = Attribute.Find("damage"),
                        Type = AttributeModifierType.Multiply,
                        Amount = damageBoost.multiplier
                    });
                }
                if (damageBoost.fixedAmount != 0)
                {
                    AttributeHolder.AddModifier(new AttributeModifier
                    {
                        Tag = "damage",
                        Attribute = Attribute.Find("damage"),
                        Type = AttributeModifierType.Add,
                        Amount = damageBoost.fixedAmount
                    });
                }
                damageBoost.duration--;
                gameStateManager.currentSaveGame.inventory.damageBoosts[i] = damageBoost;
            }
            else
            {
                Debug.Log("Removing damage boost: '"+damageBoost+"' due to the duration running out");
                gameStateManager.currentSaveGame.inventory.damageBoosts.RemoveAll(boost => boost.duration <= 0);
            }
        }
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
        if(rb.velocity.sqrMagnitude > 0.1f && AudioManager.CheckPeriod(0.35f))
        {
            AudioManager.Instance.PlaySfx("WalkSound");
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
        else if (aiming && aimTime >= AttributeHolder.GetValue(Attribute.AttackCooldown))
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
        arrow.Damage = AttackDamage;

        // Apply force to the arrow to shoot it
        var arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.AddForce(direction.normalized * arrowForce, ForceMode2D.Impulse);

        // Play the shooting sound effect
        AudioManager.Instance.PlaySfx("PlayerShoot");
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
        InGameManager.Instance.EndGame(false);
    }

    [AtomicCommand("Player", "Heal", "Heal the player to max health")]
    public void HealCommandCallback()
    {
        Heal(float.PositiveInfinity);
    }
}