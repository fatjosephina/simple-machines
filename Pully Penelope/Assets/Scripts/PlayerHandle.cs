using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandle : Handle
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyMovement>().playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyMovement>().playerInRange = false;
        }
    }
}
