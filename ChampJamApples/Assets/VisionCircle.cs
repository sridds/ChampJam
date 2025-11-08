using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VisionCircle : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float stationaryDurationMin;
    [SerializeField] float stationaryDurationMax;
    [SerializeField] Vector2 areaBounds;
    [SerializeField] Vector2 areaCenter;

    [SerializeField] float damageCooldown;
    [SerializeField] LayerMask playerLayer;

    [SerializeField] float _minOpacityRadius = 3.0f;
    [SerializeField] float _maxOpacityRadius = 1.0f;
    [SerializeField] SpriteRenderer _visionOutline;
    [SerializeField] float _flickerInterval;
    [SerializeField] int _flickerCount;
    [SerializeField] AudioClip _flickerClip;

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

    bool flag;
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, GameManager.instance.playerRef.transform.position);
        float distanceClamped = Mathf.Clamp(distance, _maxOpacityRadius, _minOpacityRadius);
        float normDistanceClamped = (distanceClamped - _maxOpacityRadius) / (_minOpacityRadius - _maxOpacityRadius);
        float t = Mathf.Lerp(1.0f, 0.0f, normDistanceClamped);

        if (GameManager.instance.playerRef.isActiveAndEnabled)
        {
            _visionOutline.color = new Color(255, 0, 0, t);
            flag = false;
        }
        else if (!flag)
        {
            _visionOutline.DOKill(false);
            _visionOutline.DOFade(0.0f, 0.3f);
            flag = true;
        }

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
