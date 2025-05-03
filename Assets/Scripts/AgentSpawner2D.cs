using UnityEngine;

public class AgentSpawner2D : MonoBehaviour
{
    [SerializeField] float spawnRadius = 10;
    [SerializeField] SpeciesManager speciesManager;

    void Awake() {
        foreach (Species species in speciesManager.allSpecies) {
            for (int i=0; i<species.startCount; i++) {
                Instantiate(species.prefab, Random.insideUnitCircle * spawnRadius, Quaternion.identity);
            }
        }
    }
}
