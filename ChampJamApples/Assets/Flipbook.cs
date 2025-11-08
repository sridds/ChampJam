using UnityEngine;

public class Flipbook : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Sprite[] _sprites;

    [SerializeField]
    private float _interval;

    private float timer;
    private int index;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > _interval)
        {
            timer = 0;
            index++;

            _renderer.sprite = _sprites[index % _sprites.Length];
        }
    }
}
