using UnityEditor.Build.Content;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Collider2D attackCollider;



    public float damage = 10f;

    public void AttackRight()
    {

        attackCollider.enabled = true;
    }

    public void AttackLeft()
    {

        attackCollider.enabled = true;

    }

    public void StopAttack()
    {

        attackCollider.enabled = false;
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {

            EnemyController enemy = collision.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.Health -= damage;
            }

        }            
    }

}
