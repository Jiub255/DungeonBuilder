using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] Transform dungeonEntrance;
    [SerializeField] float minSecondsBetweenSpawns = 1f;
    [SerializeField] float maxSecondsBetweenSpawns = 3f;
    [SerializeField] List<string> heroNames = new List<string>();
    float secondsBetweenSpawns;
    int randomHeroIndex;
    float timer;

    public void RandomizeHeroIndexAndSpawnTime()
    {
        secondsBetweenSpawns = Random.Range(minSecondsBetweenSpawns, maxSecondsBetweenSpawns);
        randomHeroIndex = Random.Range(0, heroNames.Count);
    }

    private void Update()
    {
        secondsBetweenSpawns -= Time.deltaTime;
        if (secondsBetweenSpawns <= 0)
        {
            SpawnHero(randomHeroIndex);
            RandomizeHeroIndexAndSpawnTime();
        }
    }

    public void SpawnHero(int heroIndex)
    {
        GameObject clone = ObjectPool.SharedInstance.GetPooledObject(heroNames[heroIndex]);

        if (clone)
        {
            clone.transform.position = dungeonEntrance.position;
            clone.SetActive(true);
        }
    }
}