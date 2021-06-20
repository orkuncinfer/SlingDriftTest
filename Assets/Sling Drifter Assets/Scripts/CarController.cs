using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    public static CarController instance;

    private CornerDetection cd;
    private CornerPoint cornerToHookSc;

    [Header("Controls")]
    public float MaxSpeed;
    public float Acceleration;
    public float Steering;
    float X;
    float Y = 1;

    
    private bool hooking = false;
    private bool rePositioning = false;

    [Header("Components to Assign")]
    public LineRenderer LR;
    public GameObject wheelTrackParticle;
    private GameObject instantiatedTrackParticle;


    
    private Rigidbody2D rb;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponentInChildren<CornerDetection>();  
    }

    private void Update()
    {

        GetInput();

        Movement();

        PositionCorrection();

        HandleLineRenderer();
        
    }

    private void Movement()
    {
        Vector2 speed = transform.up * (Y * Acceleration);

        rb.AddForce(speed * Time.deltaTime);

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));

        if (Acceleration > 0)
        {
            if (direction > 0)
            {
                rb.rotation -= X * Steering * (rb.velocity.magnitude / MaxSpeed) * Time.deltaTime;
            }
            else
            {
                rb.rotation += X * Steering * (rb.velocity.magnitude / MaxSpeed) * Time.deltaTime;
            }
        }

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.left)) * 2.0f;
        Vector2 relativeForce = Vector2.right * driftForce;

        rb.AddForce(rb.GetRelativeVector(relativeForce));


        if (rb.velocity.magnitude > MaxSpeed && !rePositioning)
        {
            rb.velocity = rb.velocity.normalized * MaxSpeed;
        }

        Debug.DrawLine(rb.position, rb.GetRelativePoint(relativeForce), Color.green);
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cd.cornerToHook != null && instantiatedTrackParticle == null)
            {
               instantiatedTrackParticle = Instantiate(wheelTrackParticle, transform.position, Quaternion.identity);
            }
        }

        if (Input.GetMouseButton(0))
        {
            hooking = true;    
            if (cd.cornerToHook != null) { 
            cornerToHookSc = cd.cornerToHook.GetComponent<CornerPoint>();


            if (cornerToHookSc.orientation == CornerPoint.Orientation.Clockwise)
                {
                    X = 1;
                    return;
                }
                if(cornerToHookSc.orientation == CornerPoint.Orientation.counterClokwise)
                {
                    X = -1;
                }
                
            }
        }

        
        if (Input.GetMouseButtonUp(0))
        {
            instantiatedTrackParticle.transform.SetParent(null);
            instantiatedTrackParticle = null;
            
            cd.cornerToHook.GetComponent<CornerPoint>()._isUsed = true;
            X = 0;
            cd.cornerToHook = null;
            cornerToHookSc = null;
            hooking = false;
            //PositionCorrection();
        }

        

    }

    private void HandleLineRenderer()
    {
        if (hooking && cd.cornerToHook != null)
        {
           if(instantiatedTrackParticle != null)
           instantiatedTrackParticle.transform.SetParent(gameObject.transform);

            LR.enabled = true;
            LR.SetPosition(0, transform.position);
            LR.SetPosition(1, cd.cornerToHook.transform.position);
        }
        else
        {
            LR.enabled = false;
        }

        
        
    }

    private void PositionCorrection()
    {
        if (!hooking)
        {
            float t = 0f;
            t += 10f * Time.deltaTime;

            if (rb.velocity.x > MaxSpeed / 2) // facing right
                rb.rotation = Mathf.Lerp(rb.rotation, -90, t);

            if (rb.velocity.x < -MaxSpeed / 2) // facing left
                rb.rotation = Mathf.Lerp(rb.rotation, 90, t);

            if (rb.velocity.y > (MaxSpeed / 2) + 0.5f) // facing up
                rb.rotation = Mathf.Lerp(rb.rotation, 0, t);

            if (rb.velocity.y < -MaxSpeed / 2 + 0.5f) // facing down
                rb.rotation = Mathf.Lerp(rb.rotation, -180, t);
        }
        
    }

    public void Die()
    {
        SceneManager.LoadScene(0);
    }

    public void Finish()
    {
        Time.timeScale = 0;
        print("game win");
    }

   
}
