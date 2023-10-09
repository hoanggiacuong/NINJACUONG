using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform aPoint, bPoint;
    [SerializeField] private float speed = 20f;
    Vector3 target;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = aPoint.position;
        target = bPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if (Vector2.Distance(transform.position, bPoint.position )< 0.1f)
        {
            target = aPoint.position;
        }
        else if(Vector2.Distance(transform.position, aPoint.position) < 0.1f)
        {
            target = bPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            Debug.Log("isGrd=true");
            if (transform.position.y < collision.transform.position.y)
            {
                collision.transform.SetParent(this.transform);
            }
            
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
            Debug.Log("ko vc voi mf");
        }
    }
    
}
