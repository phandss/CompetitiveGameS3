using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireBallAbility : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform spawnPoint;
    public float abilityCooldown = 2f;

    [Header("Fireball Settings")]
    public float speed = 10f;
    public float explosionRadius = 2f;
    public float detectionRadius = 2f;
    public float travelTime = 8;
    public float explosionDamage = 20f;
    public float burnDamage = 5f;
    public float burnDuration = 4f;
    public float burnTickRate = 1f;

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;
    

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }


    public bool TryExecuteAbility(Vector2 spawnPosition, Vector2 targetPosition)
    {
        if(isOnCooldown)
        {
            return false;
        }

        if(fireballPrefab == null)
        {
            Debug.Log("No Prefab Assigned for Fireball Ability");
            return false;
        }

        Vector2 direction = (targetPosition - spawnPosition).normalized;

        ExecuteAbility(spawnPosition, targetPosition);

        isOnCooldown = true;
        cooldownTimer = abilityCooldown;

        return true;
    }

    private void ExecuteAbility(Vector2 spawnPosition, Vector2 targetPosition)
    {
        GameObject fireballInstance = Instantiate(fireballPrefab, spawnPoint.position, Quaternion.identity);
        
        FireballProjectile projectile = fireballInstance.GetComponent<FireballProjectile>();

        projectile.Initialize(targetPosition);
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public float GetCooldownRemaining()
    {
        return Mathf.Max(0f, cooldownTimer);
    }
}
