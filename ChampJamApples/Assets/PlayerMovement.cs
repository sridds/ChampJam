using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float movementSpeed;
    [SerializeField] float manualSlowDownMultiplier;
    [SerializeField] MarkerManager myMarkerManager;
    [SerializeField] WormSegments wormSegments;

    [SerializeField] float slowSpeed;
    [SerializeField] float slowdownThreshold;
    [SerializeField] float maxInputtableVelocity;
    [SerializeField] float distanceUntilCanEnterApple;
    [SerializeField] float disableGravityVelocityThreshold;
    [SerializeField] float gravity;
    [SerializeField] float maxFallSpeed;

    Apple previousApple;
    public Action OnLaunch;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (previousApple != null)
        {
            float distanceFromPreviousApple = Vector2.Distance(transform.position, previousApple.transform.position);
            if (distanceFromPreviousApple > distanceUntilCanEnterApple)
            {
                previousApple = null;
            }
        }

        Gravity();
    }

    void Gravity()
    {
        Vector2 velocityMinusUp = rb.linearVelocity;
        velocityMinusUp = new Vector2(velocityMinusUp.x, Mathf.Clamp(velocityMinusUp.y, Mathf.NegativeInfinity, 0));
        if (velocityMinusUp.magnitude > disableGravityVelocityThreshold)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
        }

        if (rb.linearVelocityY <= maxFallSpeed)
        {
            rb.gravityScale = 0;
        }
    }

    private void FixedUpdate()
    {
        ManualMovement();
    }

    public void Launch(Vector2 launch, Vector2 startPos)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = startPos;
        myMarkerManager.ClearMarkerList();
        rb.AddForce(launch, ForceMode2D.Impulse);
    }

    public void ManualMovement()
    {
        float horizontalForce = movementSpeed * Input.GetAxisRaw("Horizontal");

        //If trying to manually speed up past the max, don't. (RIGHT)
        if (rb.linearVelocity.x > maxInputtableVelocity)
        {
            if (horizontalForce > 0)
            {
                horizontalForce = 0;
            }
        }

        //If trying to manually speed up past the max, don't. (LEFT)
        if (rb.linearVelocity.x < -maxInputtableVelocity)
        {
            if (horizontalForce < 0)
            {
                horizontalForce = 0;
            }
        }

        if (horizontalForce < 0 && rb.linearVelocity.x > 0)
        {
            Debug.Log("true");
            horizontalForce *= manualSlowDownMultiplier;
        }

        if (horizontalForce > 0 && rb.linearVelocity.x < 0)
        {
            Debug.Log("true");
            horizontalForce *= manualSlowDownMultiplier;
        }

        Vector3 force = new Vector2(horizontalForce, 0);

        //Add Manual Force
        rb.AddForce(force, ForceMode2D.Force);

        //Slow Down
        if (Mathf.Abs(rb.linearVelocity.x) > slowdownThreshold)
        {
            rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, 0, slowSpeed * Time.fixedDeltaTime), rb.linearVelocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            if (collision.gameObject.TryGetComponent(out Apple appleScript))
            {
                //No double dipping
                if (previousApple != null)
                {
                    return;
                }

                appleScript.Enter(transform.position);

                transform.position = appleScript.transform.position;
                myMarkerManager.ClearMarkerList();
                wormSegments.Clear();

                previousApple = appleScript;
                gameObject.SetActive(false);
            }
        }
    }
}
