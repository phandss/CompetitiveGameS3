using System;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float travelTime = 8f;
    public float explosionRadius = 2f;
    public float explosionDamage = 20f;
    public float burnDamage = 5f;
    public float burnDuration = 4f;
    public float burnTickRate = 1f;

    private Vector2 direction;
    private float travelTimer;
    private bool hasExploded = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.1f;

    }


    public void Initialize(Vector2 moveDirection)
    {
        direction = moveDirection.normalized;

        
        if(direction != Vector2.zero)
        {
                       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }

        rb.linearVelocity = direction * speed;
    }

    private void Update()
    {
        if(hasExploded)
        {
            return;
        }

        travelTimer += Time.deltaTime;

        if(travelTimer >= travelTime)
        {
            Explode();
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded)
        {
            return;
        }


        if (collision.gameObject.CompareTag("Enemy"))
        {
            ApplyBurn(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasExploded)
        {
            return;
        }
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            Explode();
        }
    }


    void ApplyBurn(GameObject enemy)
    {
        float elapsed = 0f;

        while (elapsed < burnDuration && enemy != null)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.ApplyBurnDamage(burnDamage, burnDuration, burnTickRate);
            }
        }
    }

    public void Explode()
    {
        if (hasExploded)
        {
            return;
        }
        hasExploded = true;

        rb.linearVelocity = Vector2.zero;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyController enemyController = hitCollider.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.Health -= explosionDamage;
                    
                }
            }
        }

        Destroy(gameObject);
    }
}
