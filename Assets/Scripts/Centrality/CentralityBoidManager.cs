using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralityBoidManager : BoidMan2D
{
    protected override void Update() {
        UpdateSpecies(Boid2D.allBoids);
    }
}
