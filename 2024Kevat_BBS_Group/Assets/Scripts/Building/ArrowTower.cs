using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    public float shootWait;
    public float damage;
    public Arrow arrowPrefab;
    public Transform shootPoint;
    public float arrowForce;
    private float lastShoot;
    private Building building;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        building = GetComponent<Building>();
        player = EnemyManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        var nearest = EnemyManager.instance.GetNearestEnemy(building.GetPosition());
        if (!(Time.time - lastShoot >= shootWait) || nearest == null) return;

        var relativeTarget = nearest.transform.position - transform.position;
        var angle = Mathf.Atan2(relativeTarget.y, relativeTarget.x) * Mathf.Rad2Deg;

        // Instantiate the arrow prefab at the shoot point with the correct rotation
        var arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
        arrow.Owner = player;
        arrow.Damage = damage;

        // Apply force to the arrow to shoot it
        var arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.AddForce(relativeTarget.normalized * arrowForce, ForceMode2D.Impulse);

        // Play the shooting sound effect
        AudioManager.Instance.PlayStop("PlayerShoot");
        lastShoot = Time.time;
    }
}
