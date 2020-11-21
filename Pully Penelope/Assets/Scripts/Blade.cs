using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") /*|| collision.gameObject.CompareTag("Enemy")*/)
        {
            collision.gameObject.GetComponent<Death>().isDead = true;
        }
    }
}
