using DG.Tweening;
using UnityEngine;

public class ApplePicker : MonoBehaviour
{
    AppleManager appleManager;
    [SerializeField] float movementSpeed;
    [SerializeField] MetalGearSolidController metalGearSolidController;
    [SerializeField] GameObject pointsPopup;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Transform bagHolder;
    [SerializeField] Sprite shockedSprite;
    [SerializeField] Sprite defaultSprite;
    bool hasStarted = false;
    int lastDirection;
    void Start()
    {
        appleManager = AppleManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            MoveToCenter();
        }

        if (hasStarted)
        {
            MoveToApple();
        }
    }

    public void SetDirection(int flip)
    {
        if (flip == lastDirection) return; // dont call over and over

        lastDirection = flip;
        bagHolder.DOKill(false);
        bagHolder.DOLocalMoveX(0.256f * -flip, 0.3f).SetEase(Ease.OutQuad);
    }

    void MoveToCenter()
    {
        Vector2 dirToCenter = (new Vector2(0, transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
        if (Mathf.Sign(dirToCenter.x) == -1) renderer.flipX = true;

        else renderer.flipX = false;
        SetDirection((int)Mathf.Sign(dirToCenter.x));

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, transform.position.y), movementSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, new Vector2(0, transform.position.y)) < 0.4f)
        {
            hasStarted = true;
        }
    }

    void MoveToApple()
    {
        Apple closestApple = null;
        float closestDistance = Mathf.Infinity;
        foreach (Apple apple in appleManager.groundedApples)
        {
            float distance = Vector2.Distance(transform.position, apple.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestApple = apple;
            }
        }

        if (closestApple != null)
        {
            animator.SetBool("walking", true);

            Vector2 normDirToApple = (closestApple.transform.position - transform.position).normalized;
            if (Mathf.Sign(normDirToApple.x) == -1) renderer.flipX = true;
            else renderer.flipX = false;
            SetDirection((int)Mathf.Sign(normDirToApple.x));


            transform.position = Vector2.MoveTowards(transform.position, closestApple.transform.position + new Vector3(0, 0.3f, 0), movementSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestApple.transform.position) < 0.4f)
            {
                PickUpApple(closestApple);
            }
        }
        else
        {
            animator.SetBool("walking", false);
        }

    }

    void PickUpApple(Apple apple)
    {
        if (apple.currentAge == Apple.Age.ROTTEN)
        {
            metalGearSolidController.ShowAlert();
            GameManager.instance.TakeDamage();
        }
        else
        {
            GameManager.instance.GetPoints(100);
        }
        AppleManager.instance.RemoveGroundApple(apple);
        GameObject newPoints = Instantiate(pointsPopup, transform.position, Quaternion.identity);
        Destroy(newPoints, 1.5f);
        apple.GetEaten();
    }
}
