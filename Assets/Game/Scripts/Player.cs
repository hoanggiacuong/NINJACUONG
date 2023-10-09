using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character

{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private LayerMask groudLayer;
    [SerializeField] private float speed=50;
    [SerializeField] private float jumpForce=350;

    [SerializeField] private Kunai kunaiFrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private float glidingSpeed = 3f;
    [SerializeField] private bool isJumping=false;
    [SerializeField] private bool isAttack=false;
    [SerializeField] private TrailRenderer tr;
    public static bool isGrouded = true;

    private bool isGliding;
    private bool isDeath = false;
    private bool canDash=true;
   [SerializeField] private bool isDasing;
  
   
    
    
    private float initGravityScale;
    
    private float horizontal;
    private int coin=0;
    private Vector3 savePoint;

    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    // Start is called before the first frame update
    private void Awake()
    {
        //lấy giá trị số coin đã lưu
        coin = PlayerPrefs.GetInt("coin", 0);
        initGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath)
        {
            return;
        }
        isGrouded = CheckGrounded();
        //-1.0.1
        SetMove(horizontal);
       // horizontal = Input.GetAxisRaw("Horizontal");

        if (isAttack)
        {
            return;
        }
        if (isDasing)
        {
            return;
        }

        if (isGrouded)
        {
            rb.gravityScale = initGravityScale;
            isGliding = false;
            if (isJumping)
            {
                return;
            }
            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            //anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
            //attack
            if (Input.GetKeyDown(KeyCode.C))
            {
                Attack();

              
            }

            //throw
            if (Input.GetKeyDown(KeyCode.V))
            {
                Throw();
              
            }
           
        }


       // checking fall
        if(!isGrouded && rb.velocity.y < 0&& !isGliding)
        {
            
            ChangeAnim("fall");

            isJumping = false;
            Debug.Log("dang nga");

        }
        //Fly
        if (Input.GetKeyDown(KeyCode.B))
        {
           
            Fly();
        }

        //MOVE

        if (Mathf.Abs( horizontal )> 0.1f)
        {
            
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
          
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ?0:-180, 0));
        }
        else if(isGrouded)
        {
            ChangeAnim("idle");
            rb.velocity =  Vector2.zero;
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.N)&&canDash &&!isGliding)
        {
            StartCoroutine(Dash());
        }

        //savePoint
       
        

    }



    public override void OnInit()
    {
        base.OnInit();
        isDeath = false;
        isAttack = false;
        isGliding = false;
        canDash = true;

        // origin pos
        transform.position = savePoint;
        rb.gravityScale = initGravityScale;
        ChangeAnim("idle");
        DeActiveAttack();
        SavePoint();
        //ui coin
        UiManager.instance.SetCoin(coin);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f,groudLayer);
        
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.green);
        return hit.collider != null;
    }
    public void Attack()
    {
        rb.velocity = Vector2.zero;
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
        
    }
    public void Throw()
    {
        rb.velocity = Vector2.zero;
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiFrefab, throwPoint.position, throwPoint.rotation);
       

    }
    public void Jump()
    {
        if (isGrouded)
        {
            isJumping = true;
            ChangeAnim("jump");
            rb.AddForce(jumpForce * Vector2.up);
        }
       

    }
    public void Fly()
    {
        if(rb.velocity.y < 0)
        {
            ChangeAnim("fly");
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, -glidingSpeed);
            isGliding = true;
        }

    }

    public void OnDash()
    {
        if ( canDash && !isGliding)
        {
            StartCoroutine(Dash());
        }
    }

    private void ResetAttack()
    {
        isAttack = false;
        ChangeAnim("idle");
        Debug.Log("reset at");
        
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
        isAttack = false;//da sư
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
        isAttack = true;// đã sửa
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDasing = true;
        rb.gravityScale = 0;
        ChangeAnim("dask");
        if(transform.rotation==Quaternion.Euler(new Vector3(0, 0, 0)))
        {
            rb.velocity = new Vector2(dashingPower, 0);
        }
        else
        {
            rb.velocity = new Vector2(-dashingPower, 0);
        }
       
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = initGravityScale;
        isDasing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
       
    }

      
    // di chuyen bang nut nhan tren man hinh
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    //va cham 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag( "Coin"))
        {
            
            Destroy(collision.gameObject);
            coin++;
            UiManager.instance.SetCoin(coin);
            // lưu coin vào bộ nhớ
            PlayerPrefs.SetInt("coin", coin);
        }
        if (collision.CompareTag( "DeathZone"))
        {
            isDeath = true;
            ChangeAnim("die");
           
            Invoke(nameof(OnInit), 1f);
        }
    }
    internal void SavePoint()
    {
        savePoint = transform.position;
       
    }

}
