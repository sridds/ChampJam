using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Core.Easing;
using DG.Tweening;

public class CameraJuice : MonoBehaviour
{
    public static CameraJuice instance;

    public class ShakeEvent
    {
        float duration;
        float timeRemaining;

        ShakeData data;

        public ShakeData.Target target
        {
            get
            {
                return data.target;
            }
        }

        Vector3 noiseOffset;
        public Vector3 noise;

        public ShakeEvent (ShakeData data)
        {
            this.data = data;
            duration = data.duration;
            timeRemaining = duration;

            float rand = 32.0f;

            noiseOffset.x = Random.Range(0.0f, rand);
            noiseOffset.y = Random.Range(0.0f, rand);
            noiseOffset.z = Random.Range(0.0f, rand);
        }

        public void Update()
        {
            float deltaTime;

            if (data.useUnscaledTime)
            {
                deltaTime = Time.unscaledDeltaTime;
            }
            else
            {
                deltaTime = Time.deltaTime;
            }

            timeRemaining -= deltaTime;

            float noiseOffsetDelta = deltaTime * data.frequency;

            noiseOffset.x += noiseOffsetDelta;
            noiseOffset.y += noiseOffsetDelta;
            noiseOffset.z += noiseOffsetDelta;

            noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
            noise.y = Mathf.PerlinNoise(noiseOffset.x, 1.0f);
            noise.z = Mathf.PerlinNoise(noiseOffset.x, 2.0f);

            noise -= Vector3.one * 0.5f;
            noise = new Vector3(noise.x * data.amplitude.x, noise.y * data.amplitude.y, noise.z * data.amplitude.z);

            float agePercent = timeRemaining / duration;
            float easeValue = EaseManager.Evaluate(data.easing, null, agePercent, 1.0f, 1.0f, 1.0f);
            noise *= easeValue;
        }

        public bool IsAlive()
        {
            return timeRemaining > 0.0f;
        }
    }

    List<ShakeEvent> shakeEvents = new List<ShakeEvent>();
    public Vector3 localShakePosition { get; private set; }
    public Vector3 localShakeEulerAngles { get; private set; }

    [SerializeField]
    private Transform _shakeTarget;
    [SerializeField]
    private Transform _punchTarget;
    [SerializeField]
    private int _pixelsPerUnit = 16;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddShakeEvent(ShakeData data)
    {
        shakeEvents.Add(new ShakeEvent(data));
    }

    public void AddShakeEvent(Vector3 amplitude, float frequency, float duration, ShakeData.Target target = ShakeData.Target.Rotation, Ease easing = Ease.OutQuad, bool useUnscaledTime = false)
    {
        ShakeData data = new ShakeData();
        data.amplitude = amplitude;
        data.frequency = frequency;
        data.duration = duration;
        data.easing = easing;
        data.target = target;
        data.useUnscaledTime = useUnscaledTime;

        AddShakeEvent(data);
    }

    public void AddCameraPunch(float zPunchAmount, float duration, int vibrato, float elasticity)
    {
        _punchTarget.DOKill(true);
        _punchTarget.DOPunchRotation(new Vector3(0, 0, zPunchAmount), duration, vibrato, elasticity);
    }

    private void LateUpdate()
    {
        Vector3 positionOffset = Vector3.zero;
        Vector3 rotationOffset = Vector3.zero;

        for (int i = shakeEvents.Count - 1; i != -1; i--)
        {
            ShakeEvent se = shakeEvents[i]; se.Update();

            if (se.target == ShakeData.Target.Position)
            {
                positionOffset += se.noise;
            }
            else
            {
                rotationOffset += se.noise;
            }

            if (!se.IsAlive())
            {
                shakeEvents.RemoveAt(i);
            }
        }

        localShakePosition = positionOffset;
        localShakeEulerAngles = rotationOffset;

        _shakeTarget.localPosition = RoundToPixelPerfect(localShakePosition);
        _shakeTarget.localEulerAngles = localShakeEulerAngles;
    }

    public Vector3 RoundToPixelPerfect(Vector3 pos)
    {
        float x = Mathf.Round(pos.x * _pixelsPerUnit) / _pixelsPerUnit;
        float y = Mathf.Round(pos.y * _pixelsPerUnit) / _pixelsPerUnit;

        return new Vector3(x, y, pos.z);
    }
}

[System.Serializable]
public struct ShakeData
{
    public enum Target
    {
        Position,
        Rotation
    }

    public Target target;
    public Vector3 amplitude;
    public float frequency;
    public float duration;
    public bool useUnscaledTime;
    public Ease easing;
}
