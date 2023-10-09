using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject HitVFX;
    private GameObject HitVFX_Fb;
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    public void OnInit()
    {
        rb.velocity = transform.right * 5f;
        Invoke(nameof(OnDespawn), 4f);
    }
    public void OnDespawn()
    {
        Destroy(gameObject);
        Destroy(HitVFX_Fb);


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
         
            collision.GetComponent<Character>().OnHit(30f);
             HitVFX_Fb= Instantiate(HitVFX, transform.position, Quaternion.identity);
            
            OnDespawn();
        }
    }
}
