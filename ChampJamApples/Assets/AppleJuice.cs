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
    Coroutine shakeRoutine;


    void Awake()
    {
        appleRef.Shake += ShakeVisual;
        appleRef.Deattach += BreakOff;
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
            angle = (rotationInCurve.Evaluate(elapsed / rotationOutDuration) * rotationAmount);
            ResetAppleVisual();
            shakeTransform.RotateAround(rotateAroundPoint.position, transform.forward, angle * direction);
            yield return null;
            elapsed += Time.deltaTime;
        }
    }
    
    void ResetAppleVisual()
    {
        shakeTransform.eulerAngles = Vector3.zero;
        shakeTransform.localPosition = Vector3.zero;
    }
}
