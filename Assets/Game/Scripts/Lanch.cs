using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanch : MonoBehaviour
{
    [SerializeField] Transform bomFerfab;
    [SerializeField] Transform spawPoint;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] float launchForce = 1.5f;
    [SerializeField] float trajectoryTimeStep = 0.05f;
    [SerializeField] int trajectoryStepCout = 60;
     bool startBome;


    Vector2 velocity, startMousePos, currentMousePose;


    // Start is called before the first frame update
    void Start()
    {

        lineRenderer = GetComponent<LineRenderer>();
        startBome = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startBome)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lineRenderer.enabled = true;
                startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                currentMousePose = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                velocity = (startMousePos - currentMousePose);
                DrawTrajectory();
            }
            if (Input.GetMouseButtonUp(0))
            {
                Fire();
                lineRenderer.enabled = false;
            }
        } 
        
        

        


    }

    private void DrawTrajectory()
    {
        Vector3[] position = new Vector3[60];
        for (int i = 0; i < 60; i++)
        {
            float t = i * trajectoryTimeStep;
            Vector3 pos = (Vector2)spawPoint.position + velocity * t + 0.5f * Physics2D.gravity * t * t;
            position[i] = pos;
        }
        lineRenderer.positionCount = trajectoryStepCout;
        lineRenderer.SetPositions(position);
       
    }
    void Fire()
    {
        Transform pr = Instantiate(bomFerfab, spawPoint.position, Quaternion.identity);
        pr.GetComponent<Rigidbody2D>().velocity = velocity;
        Destroy(pr.gameObject, 3f);
        startBome = false;
    }

    public void Bome()
    {
        Debug.Log("da han starbom");    
        startBome = true;
    }

    private void OnDespaw() 
    {

        
     }
}
