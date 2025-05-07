using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Singleton, main access point for species data
public class SpeciesManager : MonoBehaviour
{
    public static SpeciesManager Instance { get; private set; }
    public Species selectedSpecies;
    [SerializeField] public List<Species> allSpecies;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public static List<GameObject> GetSpeciesObjects(Species species) {
        return GameObject.FindGameObjectsWithTag(species.tag).ToList();
    }
}
