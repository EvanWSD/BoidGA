using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] GameObject plantPrefab;
    [SerializeField] float minDelay;
    [SerializeField] float maxDelay;
    [SerializeField] int maxPlants = 300;
    Bounds _camBounds;
    void Start() {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        _camBounds = new Bounds(cam.transform.position, new Vector3(width, height, 0));

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() {
        while (true) {
            if (GameObject.FindGameObjectsWithTag("Plant").Length < maxPlants)
                Spawn();
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    void Spawn() {
        Vector3 center = _camBounds.center;
        float x = Random.Range(center.x - _camBounds.size.x / 2f, center.x + _camBounds.size.x / 2f);
        float y = Random.Range(center.y - _camBounds.size.y / 2f, center.y + _camBounds.size.y / 2f);
        Vector3 spawnPos = new Vector3(x, y, 0);
        Instantiate(plantPrefab, spawnPos, Quaternion.identity);
    }
}
