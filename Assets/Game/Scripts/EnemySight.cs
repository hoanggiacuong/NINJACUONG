using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public Enemy enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.SetTarget(collision.GetComponent<Character>());
        }
        Debug.Log("cotarget");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.SetTarget(null);
        }
    }
}
