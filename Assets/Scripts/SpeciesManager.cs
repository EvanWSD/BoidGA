using System.Collections.Generic;
using UnityEngine;

// Singleton, main access point for species data
public class SpeciesManager : MonoBehaviour
{
    public static SpeciesManager Instance { get; private set; }
    [SerializeField] public List<Species> allSpecies;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
