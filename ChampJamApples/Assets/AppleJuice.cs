using System.Collections;
using UnityEngine;

public class AppleJuice : MonoBehaviour
{
    [SerializeField] Apple appleRef;

    [Header("Rotation")]
    [SerializeField] Transform shakeTransform;
    [SerializeField] Transform rotateAroundPoint;
    [SerializeField] AnimationCurve rotationOutCurve;
    [SerializeField] float rotationOutDuration;
    [SerializeField] AnimationCurve rotationInCurve;
    [SerializeField] float rotationInDuration;
    [SerializeField] float rotationAmount;
    [SerializeField] ParticleSystem shakeParticle;
    [SerializeField] float particlePlayChance;
    Coroutine shakeRoutine;

    [Header("Enter")]
    [SerializeField] ParticleSystem enterParticle;

    [Header("Falling Animation")]
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Sprite _spriteFlicker;
    [SerializeField] Sprite[] _fallSprites;
    [SerializeField] float _frameInterval;

    [Header("Worm Inside")]
    [SerializeField] SpriteRenderer _wormInside;
    [SerializeField] Sprite[] _wormPokeAnimation;
    [SerializeField] float _wormPokeInterval;

    float timer = 0.0f;
    float wormAnimTimer;

    void Awake()
    {
        appleRef.Shake += ShakeVisual;
        appleRef.Deattach += BreakOff;
        appleRef.Attach += AppleEnterEffects;
        appleRef.WormLaunched += HideWorm;
    }

    // Update is called once per frame
    void ShakeVisual(Apple.ShakeDirections shakeDir)
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        int direction = 0;
        if (shakeDir == Apple.ShakeDirections.LEFT)
        {
            direction = -1;
        }
        if (shakeDir == Apple.ShakeDirections.RIGHT)
        {
            direction = 1;
        }

        if (Random.Range(0.0f, 100.0f) <= particlePlayChance)
        {
            shakeParticle.Play();
        }

        shakeRoutine = StartCoroutine(RotateAround(direction));
    }

    void BreakOff()
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        _renderer.sprite = _spriteFlicker;

        ResetAppleVisual();
        breakoffFlag = true;
    }

    bool breakoffFlag = false;
    int index;
    private void Update()
    {
        if (breakoffFlag)
        {
            timer += Time.deltaTime;

            if(timer > _frameInterval)
            {
                index++;
                _renderer.sprite = _fallSprites[index % _fallSprites.Length];
                timer = 0.0f;
            }
        }
    }

    IEnumerator RotateAround(int direction)
    {
        HideWorm();

        float elapsed = 0;
        float angle = 0;
        

        while (elapsed < rotationOutDuration)
        {
            angle = (rotationOutCurve.Evaluate(elapsed / rotationOutDuration) * rotationAmount);
            ResetAppleVisual();
            shakeTransform.RotateAround(rotateAroundPoint.position, transform.forward, angle * direction);
            yield return null;
            elapsed += Time.deltaTime;
        }

        elapsed = 0;
        while (elapsed < rotationInDuration)
        {
            angle = (rotationInCurve.Evaluate(1 - elapsed / rotationInDuration) * rotationAmount);
            ResetAppleVisual();
            shakeTransform.RotateAround(rotateAroundPoint.position, transform.forward, angle * direction);
            yield return null;
            elapsed += Time.deltaTime;
        }
    }

    void AppleEnterEffects(Vector2 enterPosition, Vector2 enterVelocity)
    {
        Vector2 enterDirection = enterVelocity.normalized;
        //Vector2 enterDirection = ((Vector2)transform.position - enterPosition).normalized;

        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        if (enterDirection.x > 0)
        {
            shakeRoutine = StartCoroutine(RotateAround(1));
        }
        if (enterDirection.x < 0)
        {
            shakeRoutine = StartCoroutine(RotateAround(-1));
        }

        enterParticle.Play();
        StartCoroutine(IWormAnimation(enterDirection));
    }

    private IEnumerator IWormAnimation(Vector2 direction)
    {
        _wormInside.enabled = true;
        _wormInside.transform.localPosition = Vector2.zero + (Mathf.Sign(direction.x) * new Vector2(0.3f, 0.0f));
        _wormInside.flipX = Mathf.Sign(direction.x) == -1;

        for (int i = 0; i < _wormPokeAnimation.Length; i++)
        {
            _wormInside.sprite = _wormPokeAnimation[i];
            yield return new WaitForSeconds(_wormPokeInterval);
        }
    }

    void HideWorm()
    {
        _wormInside.enabled = false;
    }

    void ResetAppleVisual()
    {
        shakeTransform.eulerAngles = Vector3.zero;
        shakeTransform.localPosition = Vector3.zero;
    }
}
