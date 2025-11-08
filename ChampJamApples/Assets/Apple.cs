using UnityEngine;

public class Apple : MonoBehaviour
{
    [HideInInspector] public bool doesHaveWorm;
    [HideInInspector] public bool isAttached;
    PlayerMovement wormRef;
    GameManager manager;

    Vector2 dragDistance;
    [SerializeField] float maxDragDistance;
    [SerializeField] float launchMultiplier;

    [Header("Arrow")]
    [SerializeField] GameObject arrow;
    [SerializeField] float maxArrowLength;
    [SerializeField] float minArrowLength;

    void Start()
    {
        manager = GameManager.instance;
        wormRef = manager.playerRef;
    }

    // Update is called once per frame
    void Update()
    {
        if (doesHaveWorm)
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
                arrow.transform.localScale = Vector3.Lerp(minScale, maxScale, dragDistance.magnitude/maxDragDistance);
            }
            else
            {
                arrow.SetActive(false);
            }

            if (Input.GetMouseButtonUp(0))
            {
                WormLaunch();
            }
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    void WormShake()
    {

    }

    void WormLaunch()
    {
        doesHaveWorm = false;
        Debug.Log(dragDistance);
        Debug.Log(dragDistance * launchMultiplier);
        wormRef.Launch(dragDistance * launchMultiplier, transform.position);
    }

    public void Enter()
    {
        doesHaveWorm = true;
    }
}
