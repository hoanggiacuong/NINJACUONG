using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject HitVFX;
    private GameObject HitVFX_Fb;
    // Start is called before the first frame update
    public void OnInit()
    {

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
           
             collision.GetComponent<Character>().OnHit(20f);
             HitVFX_Fb= Instantiate(HitVFX, transform.position, Quaternion.identity);
            OnDespawn();
        }
    }
}
