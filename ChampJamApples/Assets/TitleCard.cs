using UnityEngine;

public class TitleCard : MonoBehaviour
{
    public SpriteRenderer _cardRenderer;
    public Sprite a;
    public Sprite b;
    public float inteveral = 0.4f;

    bool flag;
    float timer;

    public void Update()
    {
        timer += Time.deltaTime;

        if(timer > inteveral)
        {
            flag = !flag;

            if (flag)
            {
                _cardRenderer.sprite = a;
            }else
            {
                _cardRenderer.sprite = b;
            }

            timer = 0.0f;
        }
    }
}
