using UnityEngine;

public class WormVisuals : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _body;
    [SerializeField]
    private SpriteRenderer _wormHeadRenderer;

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
}
