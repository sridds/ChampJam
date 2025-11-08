using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MetalGearSolidController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _moveHeight = 0.5f;
    [SerializeField] private AudioClip _alertClip;

    public void ShowAlert()
    {
        StartCoroutine(IShowAlert());
    }

    private IEnumerator IShowAlert()
    {
        if (_alertClip != null) AudioManager.instance.PlaySound(_alertClip, 1.0f, 1.0f);
        _renderer.transform.localPosition = Vector3.zero;
        _renderer.DOKill(true);
        _renderer.color = Color.white;
        yield return _renderer.transform.DOLocalMoveY(_moveHeight, 0.3f).SetEase(Ease.OutQuad).WaitForCompletion();
        _renderer.DOFade(0.0f, 0.3f);
    }
}
