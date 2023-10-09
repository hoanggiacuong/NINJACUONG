using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : iState
{
    float timer;
    float randomeTime;
    public void OnEnter(Enemy enemy)
    {
        enemy.StopMoving();
        timer = 0;
        randomeTime = Random.Range(2f, 4f);
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer > randomeTime)
        {
            enemy.ChangeState(new PatrolState());  
        }

    }

    public void OnExit(Enemy enemy)
    {

    }
}