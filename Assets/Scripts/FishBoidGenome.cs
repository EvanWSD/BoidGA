using UnityEngine;

[RequireComponent(typeof(FishBoid2D))]
public class FishBoidGenome : BoidGenome
{
    protected override void AssignFromGenes() {
        base.AssignFromGenes();
        ((FishBoid2D)boid).fearWeight = genes["fearWeight"].value;
    }

    protected override void InheritDefaultGenes() {
        base.InheritDefaultGenes();
        genes["fearWeight"] = new GeneData(settings.fearWeight, 0, 20f, 3f);
    }
}
