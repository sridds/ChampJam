using UnityEngine;

public class TrailSegment : MonoBehaviour
{
    [Header("Trail")]
    [SerializeField] private Transform _trailTarget;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _speed;
    [SerializeField] private float _followDistance;

    private float currentDirection;

    private void FixedUpdate()
    {
        Vector3 followPos = _trailTarget.position - _trailTarget.up * _followDistance;
        Vector3 targetDir = (followPos - transform.position.normalized).normalized;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;

        currentDirection = Mathf.MoveTowardsAngle(currentDirection, angle, _turnSpeed);
        transform.eulerAngles = new Vector3(0, 0, currentDirection);
        transform.position += transform.up * _speed * Time.fixedDeltaTime;
    }
}
