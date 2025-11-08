using System.Collections.Generic;
using UnityEngine;

public class AppleManager : MonoBehaviour
{
    public static AppleManager instance;
    [SerializeField] Apple applePrefab;
    [SerializeField] List<AppleSpawn> appleSpawns;
    [SerializeField] float startSpawnDelay;
    [SerializeField] float endSpawnDelay;
    [SerializeField] float timeUntilEnd;
    float spawnDelay;
    float spawnTimer;
    float playtime =0 ;

    int numAttachedApples = 0;

    [System.Serializable]
    public class AppleSpawn
    {
        public Transform spawnPoint;
        public Apple currentApple;
    }

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

    void Start()
    {
        foreach (AppleSpawn appleSpawn in appleSpawns)
        {
            if (appleSpawn.currentApple != null)
            {
                numAttachedApples++;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        ControlAppleSpawn();
        ControlAppleSpawnTime();
    }

    void ControlAppleSpawn()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnDelay)
        {
            SpawnApple();
            spawnTimer = 0;
        }

        if (numAttachedApples < 2)
        {
            SpawnApple();
        }
    }

    void SpawnApple()
    {
        List<AppleSpawn> possibleSpawns = new();
        foreach(AppleSpawn appleSpawn in appleSpawns)
        {
            if (appleSpawn.currentApple == null)
            {
                possibleSpawns.Add(appleSpawn);
            }
        }

        if (possibleSpawns.Count == 0)
        {
            return;
        }

        int randomSpawn = Random.Range(0, possibleSpawns.Count);
        Apple newApple = Instantiate(applePrefab, possibleSpawns[randomSpawn].spawnPoint.position, Quaternion.identity);
        newApple.Spawn(possibleSpawns[randomSpawn]);
        possibleSpawns[randomSpawn].currentApple = newApple;
        numAttachedApples++;
    }

    public void detachApple(AppleSpawn theSpawn)
    {
        theSpawn.currentApple = null;
        numAttachedApples--;
    }

    void ControlAppleSpawnTime()
    {
        playtime += Time.deltaTime;
        spawnDelay = Mathf.Lerp(startSpawnDelay, endSpawnDelay, playtime / timeUntilEnd);
    }
}
