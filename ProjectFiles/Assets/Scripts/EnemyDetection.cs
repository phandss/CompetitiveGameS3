using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Collider2D col;
    

    public List<Collider2D> detectedEnemies = new List<Collider2D>();




    private void Start()
    {
        col.GetComponent<Collider2D>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if(collision.CompareTag("Player"))
        {
            detectedEnemies.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedEnemies.Remove(collision);
    }
}
