using UnityEngine;

public class OffscreenWarning : MonoBehaviour
{
    GameObject playerRef;
    [SerializeField] float screenTop;

    [SerializeField] GameObject visuals;
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

        transform.position = new Vector2(playerRef.transform.position.x, transform.position.y);
    }
}
