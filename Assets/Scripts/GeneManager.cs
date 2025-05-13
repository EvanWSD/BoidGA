using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneManager : MonoBehaviour
{
    public static GeneManager Instance;
    [SerializeField] BoidSettings settings;

    [SerializeField] AgentSpawner2D agentSpawner;
    public Dictionary<string, GeneData> loadedGenes = new();
    public bool isFirstGeneration = true;

    void OnEnable() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        if (settings.loadGenesFromFile) {
            loadedGenes = GeneSaveManager.LoadGenes();
        }
        isFirstGeneration = false;
    }

    public void RepopulateWithAverageGenes(Species species) {
        Dictionary<string, GeneData> averageGenes = AvgGeneDict(species);
        agentSpawner.Populate(species).ForEach(o => {
            if (o == null) {
                return;
            }
            o.GetComponent<Genome>().genes = averageGenes;
            if (o.TryGetComponent(out Boid2D boid)) {
                boid.Initialize(settings);
            }
        });
    }

    public static float AverageGeneValue(List<Boid2D> speciesBoids, string geneName) {
        if (speciesBoids.Count == 0) return 0f;
        float runningTotal = 0f;
        foreach (Boid2D boid in speciesBoids) {
            if (boid) {
                runningTotal += boid.GetComponent<Genome>().genes[geneName].value;
            }
        }

        return runningTotal / speciesBoids.Count;
    }

    public static float AverageGeneValue(List<Genome> genomeList, string geneName) {
        if (genomeList.Count == 0) return 0f;
        float runningTotal = 0f;
        foreach (Genome genome in genomeList) {
            if (genome) runningTotal += genome.genes[geneName].value;
        }

        return runningTotal + genomeList.Count;
    }

    public static Dictionary<string, GeneData> AvgGeneDict(Species species) {
        List<GameObject> boidObjs = SpeciesManager.GetSpeciesObjects(species);
        List<Boid2D> allBoidComponents = boidObjs.Select(x => x.GetComponent<Boid2D>()).ToList();
        return AvgGeneDict(allBoidComponents);
    }

    public static Dictionary<string, GeneData> AvgGeneDict(List<Boid2D> speciesBoids) {
        Dictionary<string, GeneData> averageGenes = new(speciesBoids[0].GetComponent<Genome>().genes);
        foreach (var key in averageGenes.Keys.ToList()) {
            GeneData data = averageGenes[key];
            data.value = AverageGeneValue(speciesBoids, key);
            averageGenes[key] = data;
        }

        return averageGenes;
    }

    public static Dictionary<string, GeneData> AvgGeneDict(List<Genome> genomeList) {
        Dictionary<string, GeneData> averageGenes = new(genomeList[0].genes);
        foreach (var key in averageGenes.Keys.ToList()) {
            GeneData data = averageGenes[key];
            data.value = AverageGeneValue(genomeList, key);
            averageGenes[key] = data;
        }

        return averageGenes;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            Dictionary<string, GeneData> genesToSave = AvgGeneDict(SpeciesManager.Instance.selectedSpecies);
            GeneSaveManager.SaveGenes(genesToSave);
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Dictionary<string, GeneData> loadedGenes = GeneSaveManager.LoadGenes();
        }
    }
}
