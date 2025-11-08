using System.Collections;
using UnityEngine;

public class WormVisuals : MonoBehaviour
{
    public enum EWormVisualState
    {
        Default,
        Chewing
    }

    [Header("References")]
    [SerializeField]
    private Rigidbody2D _body;
    [SerializeField]
    private SpriteRenderer _wormHeadRenderer;
    [SerializeField]
    private WormSegments _segments;
    [SerializeField]
    private PlayerMovement _movement;

    [Header("Audio")]
    [SerializeField]
    private AudioSource _source;
    [SerializeField]
    private AudioClip _munchClip;

    [Header("Sprite Animations")]
    [SerializeField]
    private Sprite _defaultSprite;
    [SerializeField]
    private Sprite[] _chewSprites;
    [SerializeField]
    private Sprite _swallowSpriteA;
    [SerializeField]
    private Sprite _swallowSpriteB;
    [SerializeField]
    private int _chewCycles = 4;
    [SerializeField]
    private float _chewFrameDuration = 0.15f;
    [SerializeField]
    private float _swallowFrameDuration = 0.2f;

    private EWormVisualState state;
    private Coroutine eatCoroutine;

    private void Start()
    {
        _movement.OnLaunch += PlayEatAnimation;
    }

    private void Update()
    {
        if(_body.linearVelocity.x > 0.0f)
        {
            _wormHeadRenderer.flipX = false;
        }
        else if(_body.linearVelocity.x < 0.0f)
        {
            _wormHeadRenderer.flipX = true;
        }
    }

    public void PlayEatAnimation()
    {
        state = EWormVisualState.Chewing;

        if (eatCoroutine != null) StopCoroutine(eatCoroutine);
        eatCoroutine = StartCoroutine(IEatAnimation());
    }

    private IEnumerator IEatAnimation()
    {
        for (int i = 0; i < _chewCycles; i++)
        {
            _source.PlayOneShot(_munchClip);
            for(int j = 0; j < _chewSprites.Length; j++)
            {
                _wormHeadRenderer.sprite = _chewSprites[j];
                yield return new WaitForSeconds(_chewFrameDuration);
            }
        }

        _wormHeadRenderer.sprite = _swallowSpriteA;
        yield return new WaitForSeconds(_swallowFrameDuration);
        _segments.Pulse(0.0f, 1.6f, 0.1f, DG.Tweening.Ease.Linear, DG.Tweening.Ease.OutQuad);
        _wormHeadRenderer.sprite = _swallowSpriteB;
        yield return new WaitForSeconds(_swallowFrameDuration);
        _wormHeadRenderer.sprite = _defaultSprite;

        state = EWormVisualState.Default;
        eatCoroutine = null;
    }
}
