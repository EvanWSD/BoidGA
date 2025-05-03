using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct GeneData
{
    public float value;
    public float minValue;
    public float maxValue;
    public float mutationDelta;

    public GeneData(float value, float minValue, float maxValue, float mutationDelta)
    {
        this.value = value;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.mutationDelta = mutationDelta;
    }
}

public class Genome : MonoBehaviour
{
    public Dictionary<string, GeneData> genes = new();
    public int generation = 1;

    public Dictionary<string, GeneData> Crossover(Genome partner)
    {
        Dictionary<string, GeneData> childGenes = new Dictionary<string, GeneData>();
        foreach (var key in genes.Keys)
        {
            childGenes[key] = (Random.value < 0.5f) ? genes[key] : partner.genes[key];
        }
        return childGenes;
    }

    public void TryMutate(float mutationRate)
    {
        Dictionary<string, GeneData> mutatedGenes = new Dictionary<string, GeneData>();
        foreach (var key in genes.Keys) {
            if (Random.value < mutationRate) {
                GeneData gene = genes[key];
                gene.value += Random.Range(-gene.mutationDelta, gene.mutationDelta);
                Mathf.Clamp(gene.value, gene.minValue, gene.maxValue);
                mutatedGenes[key] = gene;
            }
        }
        genes = mutatedGenes;
    }
}