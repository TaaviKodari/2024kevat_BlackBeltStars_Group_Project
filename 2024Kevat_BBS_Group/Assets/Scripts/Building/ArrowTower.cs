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

    // Start is called before the first frame update
    void Start()
    {
        building = GetComponent<Building>();
    }

    // Update is called once per frame
    void Update()
    {
        Enemy nearest = EnemyManager.instance.GetNearestEnemy(building.GetPosition());
        if (Time.time - lastShoot >= shootWait && nearest != null)
        {
            var angle = Mathf.Atan2(nearest.transform.position.y, nearest.transform.position.x) * Mathf.Rad2Deg;

            // Instantiate the arrow prefab at the shoot point with the correct rotation
            var arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
            arrow.Owner = gameObject;
            arrow.Damage = damage;

            // Apply force to the arrow to shoot it
            var arrowRb = arrow.GetComponent<Rigidbody2D>();
            arrowRb.AddForce(nearest.transform.position.normalized * arrowForce, ForceMode2D.Impulse);

            // Play the shooting sound effect
            AudioManager.Instance.PlayStop("PlayerShoot");
            lastShoot = Time.time;
        }
    }
}
