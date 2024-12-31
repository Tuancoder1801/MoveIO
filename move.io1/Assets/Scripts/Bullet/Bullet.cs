using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public WeaponId weaponId;
    public Character shooter;
    public float weaponSpeed;
    public float weaponRotation;
    public float maxDistance = 10f;
    public float damage = 1f;

    private Vector3 moveDirection;
    private Vector3 startPosition;

    private bool hasHitWall = false;


    void Start()
    {
        //bulletRigidbody = GetComponent<Rigidbody>();
        startPosition = transform.position;
        moveDirection = transform.forward;
    }

    void Update()
    {
        if (!hasHitWall)
        {
            Move();
            TrackingDeactive();
        }
    }

    private void Move()
    {
        transform.position += moveDirection * weaponSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, weaponRotation * Time.deltaTime);
    }

    private void TrackingDeactive()
    {
        if (Vector3.Distance(startPosition, transform.position) > maxDistance)
        {
            PoolingManager.Instance.ReturnBullet(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(shooter.CompareTag(other.transform.root.tag) == false)
        {
            PoolingManager.Instance.ReturnBullet(this);
            shooter.AddScore();

            if(shooter is Player player)
            {
                player.AddKill();
            }

            Character character = other.transform.root.GetComponent<Character>();
            if (character != null)
            {
                character.TakeAttack(damage, shooter as Enemy);
                
            }
        }
    }
}
