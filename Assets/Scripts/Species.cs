using UnityEngine;

public enum SpeciesType
{
    None,
    Plant,
    Prey,
    Predator
}

[CreateAssetMenu(fileName = "Species", menuName = "Ecosystem/Species")]
public class Species : ScriptableObject
{
    public SpeciesType speciesType;
    public int startCount;
    public int maxCount;
    public int minCount;
    public int populationCount;
    public GameObject prefab;
    public string tag;
    public bool doEvolution = true;
}
