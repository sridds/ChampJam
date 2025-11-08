using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetHealth(int health)
    {
        healthText.text = "HP " + health;
    }

    public void SetScore(int score)
    {
        scoreText.text = "SCORE\n" + score;
    }
}
