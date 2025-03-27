using System.Collections.Generic;
using UnityEngine;

public class BoidMan2D : MonoBehaviour {

    const int threadGroupSize = 1024;

    public BoidSettings settings;
    public ComputeShader compute;
    Boid2D[] boids;
    public FishBoid2D[] allFish;
    SharkBoid2D[] allSharks;

    void Start () {
        boids = FindObjectsOfType<Boid2D>();
        foreach (Boid2D b in boids) {
            b.Initialize (settings);
        }

        allFish = FindObjectsOfType<FishBoid2D>();
        allSharks = FindObjectsOfType<SharkBoid2D>();
    }

    void Update () {
        UpdateSpecies(allFish);
        UpdateSpecies(allSharks);
    }

    List<Boid2D> GetLiveBoids(Boid2D[] speciesBoids) { // can likely be optimised w/ diff data structures
        List<Boid2D> aliveBoids = new List<Boid2D>();
        foreach (Boid2D boid in speciesBoids) {
            if (boid) {
                aliveBoids.Add(boid);
            }
        }
        return aliveBoids;
    }

    void UpdateSpecies(Boid2D[] speciesBoids) {
        List<Boid2D> aliveBoids = GetLiveBoids(speciesBoids);
        int numOfSpecies = aliveBoids.Count;
        if (numOfSpecies == 0) return;

        var boidData = new BoidData2D[numOfSpecies];
        for (int i = 0; i < numOfSpecies; i++) {
            boidData[i].position = aliveBoids[i].pos;
            boidData[i].direction = aliveBoids[i].headingDir;
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
            aliveBoids[i].avgFlockHeading = boidData[i].flockHeading;
            aliveBoids[i].centreOfFlockmates = boidData[i].flockCentre;
            aliveBoids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
            aliveBoids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

            aliveBoids[i].UpdateBoid();
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

        public static int Size {
            get {
                return sizeof (float) * 2 * 5 + sizeof (int);
            }
        }
    }
}
