using UnityEngine;
using DG.Tweening;

public class WormSegment : MonoBehaviour
{
    [SerializeField]
    private MarkerManager _myMarkerManager;
    [SerializeField]
    private SpriteRenderer[] _renderers;

    public MarkerManager myMarkerManager { get { return _myMarkerManager; } }
    private Vector3[] _renderScaleOrigin;

    private void Awake()
    {
        _renderScaleOrigin = new Vector3[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderScaleOrigin[i] = _renderers[i].transform.localScale;
        }
    }

    public void Pulse(float inTime, float strength, float outTime, Ease inEase, Ease outEase)
    {
        for(int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].transform.DOKill(false);
            _renderers[i].transform.DOScale(_renderScaleOrigin[i] * strength, inTime).SetEase(inEase);
            _renderers[i].transform.DOScale(_renderScaleOrigin[i], outTime).SetEase(outEase);
        }
    }
}
