using UnityEngine;
using Random = UnityEngine.Random;

public class AgentReproduction : MonoBehaviour
{
    [SerializeField] GameObject childPrefab;
    [SerializeField] BoidSettings boidSettings;

    float _rpUrge = 0f;
    float _rpThreshold;
    float _rpThresholdBase = 30f;
    float _rpVariance = 10f;

    void Start() {
        ResetRpTimer();
    }

    void Update() {
        _rpUrge += Time.deltaTime;
        if (_rpUrge >= _rpThreshold) {
            ReproduceAsexual();
            ResetRpTimer();
        }
    }

    void ResetRpTimer() {
        _rpUrge = 0f;
        _rpThreshold = _rpThresholdBase + Random.Range(-_rpVariance, _rpVariance);
    }

    void ReproduceAsexual() {
        GameObject child = Instantiate(childPrefab, transform.position, Quaternion.identity);
        child.GetComponent<Boid2D>().Initialize(boidSettings);
    }
}
