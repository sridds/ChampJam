using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerMovement playerRef;
    public int playerHealth;
    public int score;

    [SerializeField] public float groundheight;
    public AudioClip damageClip;
    public bool isGameOver;

    void Awake()
    {
        Time.timeScale = 1;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetMouseButtonDown(0) && isGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TakeDamage()
    {
        AudioManager.instance.PlaySound(damageClip, 1.0f, 1.0f);
        playerHealth--;
        UIManager.instance.SetHealth(playerHealth);
        CameraJuice.instance.AddShakeEvent(new Vector3(0, 8, 0), 11, 0.5f);
        StartCoroutine(FreezeFrame(0.2f));
        if (playerHealth <= 0)
        {
            Death();
        }
    }

    public IEnumerator FreezeFrame(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    public void GetPoints(int amount)
    {
        score += amount;
        UIManager.instance.SetScore(score);
    }

    void Death()
    {
        UIManager.instance.ShowEndScreen(score);
        isGameOver = true;
    }
}
