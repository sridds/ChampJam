using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] GameObject endScreen;
    private void Awake()
    {
        endScreen.SetActive(false);
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

    public void ShowEndScreen(int score)
    {
        endScreen.SetActive(true);
        endScoreText.text = "YOU SCORED\n" + score;
    }
}
