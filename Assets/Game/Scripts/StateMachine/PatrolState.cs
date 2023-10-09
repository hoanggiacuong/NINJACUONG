using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : iState
{
    float timer;
    float randomeTime;
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomeTime = Random.Range(3f, 5f);
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (enemy.Target != null)
        {
           // doi huong chay toi Player
            enemy.ChangeDirecion(enemy.Target.transform.position.x>enemy.transform.position.x); // ???
            if (enemy.IsTargetInRange())
            {
                enemy.ChangeState(new AttackState());
            }
            else
            {
                enemy.Moving();
            }
            

        }
        else
        {
            if (timer < randomeTime)
            {
                enemy.Moving();
            }

            else
            {
                enemy.ChangeState(new IdleState());
            }

        }


    }

    public void OnExit(Enemy enemy)
    {
      
    }
}
