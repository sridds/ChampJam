using System;
using UnityEngine;
using static AppleManager;

public class Apple : MonoBehaviour
{
    [HideInInspector] public bool doesHaveWorm;
    [HideInInspector] public bool isAttached = true;
    PlayerMovement wormRef;
    GameManager manager;
    [SerializeField] Rigidbody2D body;

    Vector2 dragDistance;
    [SerializeField] float maxDragDistance;
    [SerializeField] float launchMultiplier;
    [SerializeField] int health;
    [SerializeField] float fallSpeed;
    [SerializeField] float popForce;
    [SerializeField] float fallGravity;
    [SerializeField] float timeUntilOld;
    [SerializeField] float timeUntilRotten;
    [SerializeField] ParticleSystem healParticle;
    float oldTimer = 0;

    [Header("Arrow")]
    [SerializeField] GameObject arrow;
    [SerializeField] float maxArrowLength;
    [SerializeField] float minArrowLength;

    public Action<ShakeDirections> Shake;
    public Action Deattach;
    public Action<Vector2, Vector2> Attach;
    public Action WormLaunched;
    AppleSpawn spawnPoint;

    public enum Age
    {
        FRESH,
        OLD,
        ROTTEN,
    }
    public Age currentAge;

    bool isGrounded;

    public enum ShakeDirections
    {
        NONE,
        LEFT,
        RIGHT,
    }
    ShakeDirections lastShakeDirection;

    void Start()
    {
        manager = GameManager.instance;
        wormRef = manager.playerRef;
        isAttached = true;
        currentAge = Age.FRESH;
    }

    // Update is called once per frame
    void Update()
    {
        //Does Have Worm
        if (doesHaveWorm)
        {
            WormShake();
            DetectLaunch();
        }
        else
        {
            arrow.SetActive(false);
        }

        DropApple();


        if (transform.position.y <= GameManager.instance.groundheight)
        {
            if (!isGrounded)
            {
                isGrounded = true;
                AppleManager.instance.AddGroundApple(this);
            }
            body.bodyType = RigidbodyType2D.Static;
        }

        if (isAttached && !doesHaveWorm)
        {
            SetAge();
        }

        if (doesHaveWorm && !isAttached && currentAge == Age.ROTTEN)
        {
            healParticle.Play();
            oldTimer = 0;
            currentAge = Apple.Age.OLD;
        }
    }

    void SetAge()
    {
        oldTimer += Time.deltaTime;

        if (oldTimer > timeUntilOld)
        {
            currentAge = Apple.Age.OLD;
        }
        if (oldTimer > timeUntilRotten)
        {
            currentAge = Apple.Age.ROTTEN;
        }

        if (currentAge == Age.ROTTEN)
        {
            health = 0;
        }


    }

    public void Spawn(AppleSpawn spawn)
    {
        spawnPoint = spawn;
    }
    void DropApple()
    {
        if (isAttached)
        {
            body.gravityScale = 0;
        }

        if (health <= 0 && isAttached)
        {
            Debug.Log("Drop");
            //Drop Apple Effects
            isAttached = false;
            if (spawnPoint != null)
            {
                AppleManager.instance.detachApple(spawnPoint);
            }
            Deattach?.Invoke();
            body.AddForce(new Vector2(0, popForce), ForceMode2D.Impulse);
        }

        if (!isAttached && !isGrounded)
        {
            body.gravityScale = fallGravity;
            body.linearVelocityY = Mathf.Clamp(body.linearVelocityY, fallSpeed, Mathf.Infinity);
        }
    }

    void DetectLaunch()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragDistance = new Vector2(transform.position.x, transform.position.y) - mousePos;
            dragDistance = Vector2.ClampMagnitude(dragDistance, maxDragDistance);

            //Arrow
            arrow.SetActive(true);

            Vector2 direction = dragDistance.normalized;
            float arrowRotation = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
            arrow.transform.eulerAngles = new Vector3(0, 0, arrowRotation);

            Vector3 minScale = new Vector3(arrow.transform.localScale.x, minArrowLength, 0);
            Vector3 maxScale = new Vector3(arrow.transform.localScale.x, maxArrowLength, 0);
            arrow.transform.localScale = Vector3.Lerp(minScale, maxScale, dragDistance.magnitude / maxDragDistance);
        }
        else
        {
            arrow.SetActive(false);
        }


        if (Input.GetMouseButtonUp(0))
        {
            WormLaunch();
            WormLaunched?.Invoke();
        }
    }



    void WormShake()
    {
        if (isAttached)
        {
            if (Input.GetAxisRaw("Horizontal") > 0 && lastShakeDirection != ShakeDirections.RIGHT)
            {
                lastShakeDirection = ShakeDirections.RIGHT;
                Shake?.Invoke(lastShakeDirection);
                health--;
            }

            if (Input.GetAxisRaw("Horizontal") < 0 && lastShakeDirection != ShakeDirections.LEFT)
            {
                lastShakeDirection = ShakeDirections.LEFT;
                Shake?.Invoke(lastShakeDirection);
                health--;
            }
        }
    }

    void WormLaunch()
    {
        doesHaveWorm = false;
        wormRef.Launch(dragDistance * launchMultiplier, transform.position);
    }

    public void Enter(Vector3 enterPos, Vector3 enterVelocity)
    {
        Attach?.Invoke(enterPos, enterVelocity);
        doesHaveWorm = true;
    }

    public void GetEaten()
    {
        if (doesHaveWorm)
        {
            doesHaveWorm = false;
            wormRef.Bounce(transform.position, true);
        }
        Destroy(gameObject);
        
    }
}
