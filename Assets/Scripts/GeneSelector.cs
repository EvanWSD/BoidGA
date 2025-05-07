using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GeneSelector : MonoBehaviour
{
    [HideInInspector] public string selectedGene = "minSpeed";
    string lastSelection;
    [HideInInspector] public List<string> geneOptions = new() {
        "minSpeed",
        "maxSpeed",
        "maxTurnRate",
        "separationWeight",
        "alignWeight",
        "cohesionWeight",
        "perceptionRadius",
        "separationRadius",
        "fovAngle",
        "rpThresholdBase",
        "rpThresholdVariance",
        "centralityWeight",
        "fearWeight"
    };
    public UnityEvent OnSelectGene { get; } = new();

    void Start() {
        geneOptions = Boid2D.allBoids[0].GetComponent<Genome>().genes.Keys.ToList();
        selectedGene = geneOptions[0];
        lastSelection = selectedGene;
    }

    void Update() {
        if (selectedGene != lastSelection) {
            OnSelectGene.Invoke();
            lastSelection = selectedGene;
        }
    }
}