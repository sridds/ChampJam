using UnityEngine;

public class WormSegment : MonoBehaviour
{
    [SerializeField]
    private MarkerManager _myMarkerManager;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public MarkerManager myMarkerManager { get { return _myMarkerManager; } }

    public void Pulse(float strength, float returnTime)
    {

    }
}
