
using UnityEngine;

[RequireComponent(typeof(CentralBoid))]
public class CentralityBoidGenome : BoidGenome
{
    protected override void AssignFromGenes() {
        base.AssignFromGenes();
        ((CentralBoid)boid).centralityWeight = genes["centralityWeight"].value;
    }

    protected override void InheritDefaultGenes() {
        base.InheritDefaultGenes();
        genes["centralityWeight"] = new GeneData(settings.centralityWeight, 0, 1f, 0.3f);
    }
}
