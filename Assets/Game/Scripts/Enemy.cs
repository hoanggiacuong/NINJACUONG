using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed=5f;
    [SerializeField] private Rigidbody2D rb;
    

    [SerializeField] private GameObject attackArea;
    private iState currentState;
    


    private bool isRight = true;
    private Character target;
    public Character Target => target;


    private void Update()
    {
        if (currentState != null&& !IsDead)
        {
            currentState.OnExecute(this);
        }
    }
    public override void OnInit()
    {
       
        base.OnInit();
       
        
        ChangeState(new IdleState());
        DeActiveAttack();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);
        Destroy(healthBar.gameObject);
        Invoke(nameof(OnInit), 3f);
    }
    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
        
    }

    //chane state
   
    public void ChangeState(iState newState)
    {
        if(currentState!= null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void Moving()
    {
        ChangeAnim("run");
       
        rb.velocity = transform.right * moveSpeed;

    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
       
    }
    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);

    }
    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) < attackRange)
        {
            return true;
        }
        else { return false; }
       
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    internal void SetTarget(Character character) //????
    {
        this.target = character;
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyWall"))
        {
            ChangeDirecion(!isRight);
        }
    }

    public void ChangeDirecion(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(0, 180, 0));
    }
}


