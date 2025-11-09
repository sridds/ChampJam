using UnityEngine;

public class ApplePicker : MonoBehaviour
{
    AppleManager appleManager;
    [SerializeField] float movementSpeed;
    [SerializeField] MetalGearSolidController metalGearSolidController;
    [SerializeField] GameObject pointsPopup;
    void Start()
    {
        appleManager = AppleManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToApple();
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
            transform.position = Vector2.MoveTowards(transform.position, closestApple.transform.position + new Vector3(0, 0.3f,0), movementSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestApple.transform.position) < 0.4f)
            {
                PickUpApple(closestApple);
            }
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
        apple.GetEaten();
    }
}
