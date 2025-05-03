using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneManager : MonoBehaviour
{
    public static GeneManager Instance;
    [SerializeField] BoidSettings settings;
    public Dictionary<string, GeneData> loadedGenes = new();


    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        if (settings.loadGenesFromFile) {
            loadedGenes = GeneSaveManager.LoadGenes();
        }
    }
}
