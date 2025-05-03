using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Specialised per-project, for saving anything used for F(x) in CustomGraph.cs
public class GraphFuncLib : MonoBehaviour
{
    public float AverageGeneValue(List<Boid2D> speciesBoids, string geneName) {
        if (speciesBoids.Count == 0) return 0f;
        float runningTotal = 0f;
        foreach (Boid2D boid in speciesBoids) {
            if (boid) {
                runningTotal += boid.GetComponent<Genome>().genes[geneName].value;
            }
        }
        return runningTotal / speciesBoids.Count;
    }

    public List<float> GetAllAverageGenes(List<Boid2D> speciesBoids) {
        List<float> averageGenes = new();
        foreach (var gene in speciesBoids[0].GetComponent<Genome>().genes) {
            averageGenes.Add(AverageGeneValue(speciesBoids, gene.Key));
        }
        return averageGenes;
    }

    public Dictionary<string, GeneData> GeneListToDict(List<float> geneList, List<Boid2D> speciesBoids) {
        Dictionary<string, GeneData> templateDict = new Dictionary<string, GeneData>(speciesBoids[0].GetComponent<Genome>().genes);
        foreach (var kvp in templateDict) {
            GeneData gene = kvp.Value;
            gene.value = geneList[templateDict.Keys.ToList().IndexOf(kvp.Key)];
            templateDict[kvp.Key] = gene;
        }
        return templateDict;
    }

    public Dictionary<string, GeneData> AvgGeneDict(List<Boid2D> speciesBoids) {
        Dictionary<string, GeneData> averageGenes = new(speciesBoids[0].GetComponent<Genome>().genes);
        foreach (var key in averageGenes.Keys.ToList()) {
            GeneData data = averageGenes[key];
            data.value = AverageGeneValue(speciesBoids, key);
            averageGenes[key] = data;
        }
        return averageGenes;
    }

    public float Population(List<Boid2D> speciesBoids) {
        return speciesBoids.Count;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            Dictionary<string, GeneData> genesToSave = AvgGeneDict(FishBoid2D.allFish);
            GeneSaveManager.SaveGenes(genesToSave);
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Dictionary<string, GeneData> loadedGenes = GeneSaveManager.LoadGenes();
            Debug.Log(loadedGenes["minSpeed"].value);
        }
    }
}
