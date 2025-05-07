using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner2D : MonoBehaviour
{
    [SerializeField] float spawnRadius = 10;
    [SerializeField] SpeciesManager speciesManager;

    void Awake() {
        foreach (Species species in speciesManager.allSpecies) {
            Populate(species, true);
        }
    }

    public List<GameObject> Populate(Species species, bool doResetPopulation = false) {
        List<GameObject> newObjects = new();
        if (doResetPopulation)
            species.populationCount = 0;
        for (int i=0; i<species.startCount; i++) {
            if (species.populationCount >=  species.maxCount) {
                return newObjects;
            }
            GameObject newAgent = Instantiate(species.prefab, Random.insideUnitCircle * spawnRadius, Quaternion.identity);
            newObjects.Add(newAgent);
            species.populationCount++;
        }
        return newObjects;
    }
}
