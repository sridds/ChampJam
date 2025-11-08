using UnityEngine;

public class VisionTriangle : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform origin;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float height = Vector2.Distance(target.position, origin.position);
        float width = target.transform.localScale.x;
        transform.localScale = new Vector2(width, height);
        transform.position = origin.transform.position;

        Vector2 angle = origin.position - target.position;
        float degrees = (Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg);
        transform.eulerAngles = new Vector3(0, 0, degrees - 90);
    }
}
