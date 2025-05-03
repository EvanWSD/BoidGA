using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;



public class AgentReproduction : MonoBehaviour
{
    [SerializeField] Species species;
    [SerializeField] BoidSettings boidSettings;

    Genome myGenome;
    Genome partner;
    PartnerDetector partnerDetector;
    float partnerCheckChance = 0.01f;
    float partnerCheckDistance = 5f;

    float _rpUrge = 0f;
    float _rpThreshold;
    [HideInInspector] public float rpThresholdBase = 5f;
    [HideInInspector] public float rpVariance = 2f;

    public UnityEvent OnDeathAttempt = new();
    public UnityEvent OnReproduce = new();

    void Start() {
        ResetRpTimer();
        species.populationCount = GameObject.FindGameObjectsWithTag(species.prefab.tag).Length;
        myGenome = GetComponent<Genome>();
        partnerDetector = GetComponentInChildren<PartnerDetector>();
        partnerDetector.OnPartnerFound.AddListener((AgentReproduction other) => {
            partner = other.GetComponent<Genome>();
            partnerDetector.available = false;
            ReproduceSexual(partner);
            ResetRpTimer();
            BreakUp();
        });
        OnDeathAttempt.AddListener(() => {
            if (species.populationCount < species.minCount) {
                return;
            }
            species.populationCount--;
            Destroy(gameObject);
        });
    }

    void Update() {
        _rpUrge += Time.deltaTime;
        if (_rpUrge > _rpThreshold && !partner) {
            partnerDetector.available = true;
        }
    }

    void ResetRpTimer() {
        _rpUrge = 0f;
        _rpThreshold = rpThresholdBase + Random.Range(-rpVariance, rpVariance);
    }

    void BreakUp() {
        partner = null; // :(
    }

    void ReproduceAsexual() {
        TryCreateChildAgent(myGenome.genes);
    }

    void ReproduceSexual(Genome partner) {
        Dictionary<string, GeneData> childGenes = myGenome.Crossover(partner);
        TryCreateChildAgent(childGenes);
    }

    void TryCreateChildAgent(Dictionary<string, GeneData> childGenes) {

        if (species.populationCount >= species.maxCount) {
            return;
        }
        GameObject child = Instantiate(species.prefab, transform.position, transform.localRotation);
        child.GetComponent<Boid2D>().Initialize(boidSettings);
        InheritGenesToChild(child.GetComponent<Genome>(), childGenes);
        species.populationCount++;
        OnReproduce.Invoke();
    }

    void InheritGenesToChild(Genome childGenome, Dictionary<string, GeneData> childGenes) {
        childGenome.genes = childGenes;
        childGenome.generation = Mathf.Max(myGenome.generation, partner.generation) + 1;
        childGenome.TryMutate(boidSettings.mutationRate);
    }
}
