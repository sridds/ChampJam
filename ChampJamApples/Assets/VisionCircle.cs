using System.Collections;
using UnityEngine;
using static AppleManager;

public class VisionCircle : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float stationaryDurationMin;
    [SerializeField] float stationaryDurationMax;
    [SerializeField] Vector2 areaBounds;
    [SerializeField] Vector2 areaCenter;

    [SerializeField] float damageCooldown;
    [SerializeField] LayerMask playerLayer;
    float cooldownTimer = 0;
    void Start()
    {
        StartCoroutine(MovePosition());
    }

    IEnumerator MovePosition()
    {
        float elapsed = 0;
        float targetPosX = Random.Range(-areaBounds.x/2, areaBounds.x/2) + areaCenter.x;
        float targetPosY = Random.Range(-areaBounds.y/2, areaBounds.y/2) + areaCenter.y;
        Vector2 targetPos = new Vector2(targetPosX, targetPosY);
        float distance = Vector2.Distance(transform.position, targetPos);
        float duration = movementSpeed * distance;
        Vector2 startPos = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
            elapsed += Time.deltaTime;
        }

        duration = Random.Range(stationaryDurationMin, stationaryDurationMax);
        yield return new WaitForSeconds(duration);
        StartCoroutine(MovePosition());
    }

    private void Update()
    {

        //Damage Player
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer > damageCooldown)
        {
            if (Physics2D.OverlapCircle(transform.position, transform.localScale.x / 2, playerLayer))
            {
                GameManager.instance.TakeDamage();
                cooldownTimer = 0;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.3f, 0.5f, 1.0f, 0.4f);
        Gizmos.DrawCube(areaCenter, areaBounds);
    }
}
