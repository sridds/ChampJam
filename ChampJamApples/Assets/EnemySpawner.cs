using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenEnemies;
    [SerializeField] int maxEnemies;
    [SerializeField] Vector3 spawnLocation;
    int currentEnemies;
    float time;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        switch (currentEnemies)
        {
            case 0:
                if (time > 5)
                {
                    Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
                    currentEnemies++;
                }
                break;
            case 1:
                if (time > timeBetweenEnemies)
                {
                    Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
                    currentEnemies++;
                }
                break;
            case 2:
                if (time > timeBetweenEnemies * 2)
                {
                    Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
                    currentEnemies++;
                }
                break;
            case 3:
                if (time > timeBetweenEnemies * 10)
                {
                    Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
                    currentEnemies++;
                }
                break;
            default:
                break;
        }
    }
}
