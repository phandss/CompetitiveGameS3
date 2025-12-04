using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 1;
    Animator animator;
    public Collider2D attackbox;
    public EnemyDetection enemyDetection;
    public float moveSpeed = 2f;
    Rigidbody2D rb;
    Vector2 direction;

    public float Health{
        set
        {
            health = value;

            if (health <= 0)
            {
                Debug.Log("Enemy died!");
                Defeated();
                // Here you can add code to handle enemy death
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

    }

    private void FixedUpdate()
    {
        if(enemyDetection.detectedEnemies.Count > 0)
        {
            direction = (enemyDetection.detectedEnemies[0].transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(10);
            }
            Debug.Log("Player took damage");
            // Here you can add code to handle player taking damage
        }
    }
}