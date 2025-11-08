using UnityEngine;

public class OffscreenWarning : MonoBehaviour
{
    GameObject playerRef;
    [SerializeField] float screenTop;

    [SerializeField] GameObject visuals;
    [SerializeField] SpriteRenderer headSpriteReference;
    [SerializeField] SpriteRenderer playerHeadReference;

    void Start()
    {
        playerRef = GameManager.instance.playerRef.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef.transform.position.y > screenTop)
        {
            visuals.SetActive(true);
        }
        else
        {
            visuals.SetActive(false);
        }

        headSpriteReference.sprite = playerHeadReference.sprite;
        transform.position = new Vector2(playerRef.transform.position.x, transform.position.y);
    }
}
