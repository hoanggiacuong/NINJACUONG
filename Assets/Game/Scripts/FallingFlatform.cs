using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFlatform : MonoBehaviour
{
    Rigidbody2D rb;
    bool platformMovingBack;
    Vector2 initPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (platformMovingBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, initPos, 20f * Time.deltaTime);


        }
        if ( Mathf.Abs(transform.position.y - initPos.y) < 0.01)
        {
            platformMovingBack = false;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Invoke("DropPlatform", 0.5f);
        }
    }
    void DropPlatform()
    {
        rb.isKinematic = false;
        Invoke("GetPlatformBack", 2f);
    }
    void GetPlatformBack()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        platformMovingBack = true;
    }
}
