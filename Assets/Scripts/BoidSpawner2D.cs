using UnityEngine;

public class BoidSpawner2D : MonoBehaviour
{
    [SerializeField] GameObject boidPrefab;
    [SerializeField] GameObject sharkPrefab;
    [SerializeField] BoidSettings settings;
    [SerializeField] int numFishToSpawn = 50;
    [SerializeField] int numSharksToSpawn = 10;
    [SerializeField] float spawnRadius = 10;

    void Awake()
    {
        for (int i=0; i<numFishToSpawn; i++) {
            Vector3 spawnPos = Random.insideUnitCircle * spawnRadius;
            FishBoid2D newFish = Instantiate(boidPrefab, spawnPos, Quaternion.identity).GetComponent<FishBoid2D>();
            if (i==0) newFish.posterBoid = true;
        }

        for (int i=0; i<numSharksToSpawn; i++) {
            Vector3 spawnPos = Random.insideUnitCircle * spawnRadius;
            Instantiate(sharkPrefab, spawnPos, Quaternion.identity).GetComponent<Boid2D>();
        }
    }
}
