using System.Collections.Generic;
using UnityEngine;

public class BoidMan2D : MonoBehaviour {

    const int threadGroupSize = 1024;

    public BoidSettings settings;
    public ComputeShader compute;

    [SerializeField] SpeciesManager speciesManager;

    void Start () {
        foreach (Boid2D b in Boid2D.allBoids) {
            b.Initialize(settings);
        }
    }

    protected virtual void Update () {
        UpdateSpecies(FishBoid2D.allFish);
        UpdateSpecies(SharkBoid2D.allSharks);
    }

    protected void UpdateSpecies(List<Boid2D> speciesBoids) {
        int numOfSpecies = speciesBoids.Count;
        if (numOfSpecies == 0) return;

        var boidData = new BoidData2D[numOfSpecies];
        for (int i = 0; i < numOfSpecies; i++) {
            boidData[i].position = speciesBoids[i].pos;
            boidData[i].direction = speciesBoids[i].headingDir;
            boidData[i].perceptionRadius = speciesBoids[i].perceptionRadius;
            boidData[i].separationRadius = speciesBoids[i].separationRadius;
        }

        var boidBuffer = new ComputeBuffer(numOfSpecies, BoidData2D.Size);
        boidBuffer.SetData(boidData);

        compute.SetBuffer(0, "boids", boidBuffer);
        compute.SetInt("numBoids", numOfSpecies);
        compute.SetFloat("viewRadius", settings.perceptionRadius);
        compute.SetFloat("avoidRadius", settings.separationRadius);

        int threadGroups = Mathf.CeilToInt(numOfSpecies / (float)threadGroupSize);
        compute.Dispatch(0, threadGroups, 1, 1);

        boidBuffer.GetData(boidData);

        for (int i = 0; i < numOfSpecies; i++) {
            speciesBoids[i].avgFlockHeading = boidData[i].flockHeading;
            speciesBoids[i].centreOfFlockmates = boidData[i].flockCentre;
            speciesBoids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
            speciesBoids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

            speciesBoids[i].UpdateBoid();
        }
        boidBuffer.Release();
    }

    public struct BoidData2D {
        public Vector2 position;
        public Vector2 direction;

        public Vector2 flockHeading;
        public Vector2 flockCentre;
        public Vector2 avoidanceHeading;
        public int numFlockmates;

        public float perceptionRadius;
        public float separationRadius;

        public static int Size {
            get {
                int sizeOfGenes = sizeof(float) * 2;
                return sizeof(float) * 2 * 5 + sizeof(int) + sizeOfGenes;
            }
        }
    }
}
