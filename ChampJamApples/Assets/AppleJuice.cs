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


    void Awake()
    {
        appleRef.Shake += ShakeVisual;
        appleRef.Deattach += BreakOff;
        appleRef.Attach += AppleEnterEffects;
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
        ResetAppleVisual();
    }

    IEnumerator RotateAround(int direction)
    {
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

    void AppleEnterEffects(Vector2 enterPosition)
    {
        Vector2 enterDirection = ((Vector2)transform.position - enterPosition).normalized;

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
    }
    void ResetAppleVisual()
    {
        shakeTransform.eulerAngles = Vector3.zero;
        shakeTransform.localPosition = Vector3.zero;
    }
}
