using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatText;

    private float hp;
    private string currentAnimName;

    public bool IsDead => hp <= 0; //nếu hp<=0 thì return IsDead=true

    void Start()
    {
        OnInit();
    }

    // Update is called once per frame

    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, this.transform);
    }

    public virtual void OnDespawn()
    {

    }


    protected virtual void OnDeath()
    {
        ChangeAnim("die");
       
        Invoke(nameof(OnDespawn), 2f);
    }
    public void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
           
        }
        else
        {
            OnDeath();
            hp = 0;
        }
        healthBar.SetNewHp(hp);
        Instantiate(combatText, this.transform.position+Vector3.up, Quaternion.identity).OnInit(damage); //????

    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }


  

}
