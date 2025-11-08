using System;
using UnityEngine;

public class SinMover : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector2 _circleAmplitude;

    [SerializeField]
    private Vector2 _circleFrequency;

    private float circleTime;


    private void Update()
    {
        circleTime += Time.deltaTime;

        _target.transform.localPosition = new Vector3(
            Mathf.Cos(circleTime * _circleFrequency.x) * _circleAmplitude.x,
            Mathf.Sin(circleTime * _circleFrequency.y) * _circleAmplitude.y);
    }
}
