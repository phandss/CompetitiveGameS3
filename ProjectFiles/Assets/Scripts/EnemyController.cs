using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float health = 1;
    Animator animator;
    BoxCollider2D attackbox;
    public EnemyDetection enemyDetection;
    public float moveSpeed = 2f;
    Rigidbody2D rb;
    Vector2 direction;
    private float damageCooldown = 1f;
    private float lastDamageTime;


    public float Health{
        set
        {
            health = value;

            if (health <= 0)
            {
                Debug.Log("Enemy died!");
                Defeated();
                
            }
        }
        get
        {
            return health;
        }

    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackbox = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if(enemyDetection.detectedEnemies.Count > 0)
        {
            direction = (enemyDetection.detectedEnemies[0].transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            
        }
        
    }

    public void Defeated()
    {
        animator.SetTrigger("die");
    }



    public void ClearEnemy()
    {
        Destroy(gameObject);
        GameManager.instance.AddScore(10);
    }

    private void OntriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();


            if (player != null)
            {
                if (Time.time - lastDamageTime < damageCooldown)
                {
                    return;
                }
                player.TakeDamage(10);
                lastDamageTime = Time.time;
            }
            Debug.Log("Player took damage");
            
        }
    }


    public void ApplyBurnDamage(float damage, float duration, float tickRate)
    {
        StartCoroutine(BurnDamageOverTime(damage, duration, tickRate));
    }

    private IEnumerator BurnDamageOverTime(float damage, float duration, float tickRate)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Health -= damage;
            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }
    }
}