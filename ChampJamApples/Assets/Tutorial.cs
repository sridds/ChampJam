using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;
    [SerializeField] GameObject mouseTutorial;
    [SerializeField] GameObject keyTutorial;
    bool hasBeenInApple;
    bool hasLaunchedFromApple;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        keyTutorial.SetActive(false);
        mouseTutorial.SetActive(false);

    }

    public void KeyTutorial()
    {
        if (!hasBeenInApple)
        {
            keyTutorial.SetActive(true);
        }
    }

    public void EndKeyTutorial()
    {
        hasBeenInApple = true;
        keyTutorial.SetActive(false);
    }

    public void MouseTutorial()
    {
        if (!hasLaunchedFromApple)
        {
            mouseTutorial.SetActive(true);
        }
    }

    public void EndMouseTutorial()
    {
        mouseTutorial.SetActive(false);
        hasLaunchedFromApple = true;
    }
}
