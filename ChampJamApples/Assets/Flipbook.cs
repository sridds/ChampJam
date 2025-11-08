using UnityEngine;

public class Flipbook : MonoBehaviour
{
    [SerializeField]
    private Vector2 _circleAmplitude;

    [SerializeField]
    private Vector2 _circleFrequency;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Sprite[] _sprites;

    [SerializeField]
    private float _interval;

    private float circleTime;
    private float timer;
    private int index;

    private void Awake()
    {
        circleTime += Random.Range(0.0f, 3.0f);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > _interval)
        {
            timer = 0;
            index++;

            _renderer.sprite = _sprites[index % _sprites.Length];
        }

        circleTime += Time.deltaTime;

        _renderer.transform.localPosition = new Vector3(
            Mathf.Cos(circleTime * _circleFrequency.x) * _circleAmplitude.x,
            Mathf.Sin(circleTime * _circleFrequency.y) * _circleAmplitude.y);
    }
}
