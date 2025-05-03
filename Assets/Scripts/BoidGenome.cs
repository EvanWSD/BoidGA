using UnityEngine;

public class BoidGenome : Genome
{
    [SerializeField] BoidSettings settings;

    Boid2D boid;
    AgentReproduction ecoAgent;

    void Start() {
        boid = GetComponent<Boid2D>();
        ecoAgent = GetComponent<AgentReproduction>();

        if (generation == 1) InheritDefaultGenes();
        else if (settings.loadGenesFromFile) genes = GeneManager.Instance.loadedGenes;
        AssignFromGenes();
    }

    void AssignFromGenes() {
        boid.minSpeed = genes["minSpeed"].value;
        boid.maxSpeed = genes["maxSpeed"].value;
        boid.maxTurnRate = genes["maxTurnRate"].value;
        boid.separationWeight = genes["separationWeight"].value;
        boid.alignWeight = genes["alignWeight"].value;
        boid.cohesionWeight = genes["cohesionWeight"].value;
        boid.perceptionRadius = genes["perceptionRadius"].value;
        boid.separationRadius = genes["separationRadius"].value;
        boid.fovAngle = genes["fovAngle"].value;

        ecoAgent.rpThresholdBase = genes["rpThresholdBase"].value;
        ecoAgent.rpVariance = genes["rpThresholdVariance"].value;
    }

    void InheritDefaultGenes() {
        // Boid Movement
        genes["minSpeed"] = new GeneData(settings.minSpeed, 0.1f, 10f, 1f);
        genes["maxSpeed"] = new GeneData(settings.maxSpeed, 0.1f, 10f, 1f);
        genes["maxTurnRate"] = new GeneData(settings.maxTurnRate, 0.1f, 0.5f, 0.1f);
        genes["separationWeight"] = new GeneData(settings.separationWeight, 0f, 5f, 0.1f);
        genes["alignWeight"] = new GeneData(settings.alignWeight, 0f, 5f, 0.1f);
        genes["cohesionWeight"] = new GeneData(settings.cohesionWeight, 0f, 5f, 0.1f);
        genes["perceptionRadius"] = new GeneData(settings.perceptionRadius, 0.1f, 10f, 0.1f);
        genes["separationRadius"] = new GeneData(settings.separationRadius, 0.1f, 10f, 0.1f);
        genes["fovAngle"] = new GeneData(settings.fovAngle, 10f, 180f, 1f);

        // Ecosystem Simulation
        genes["rpThresholdBase"] = new GeneData(settings.rpThresholdBase, 20f, 40f, 1f);
        genes["rpThresholdVariance"] = new GeneData(settings.rpThresholdVariance, 0f, 15f, 1f);
    }
}
