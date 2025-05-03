using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SharkState
{
    idle,
    chasing
}

public class SharkBoid2D : Boid2D
{
    public static List<Boid2D> allSharks = new();

    [SerializeField] LayerMask fishMask;
    FishBoid2D targetFish;
    Vector2 lastKnownTargetPos;
    float fishTargetWeight = 10f;
    SharkState state = SharkState.idle;
    public UnityEvent<FishBoid2D> onSawFish = new();

    float loseFishTimerMax = 2f;
    float loseFishTimerDelta = 2f;

    void Start() {
        onSawFish.AddListener(OnSawFish);
        debugColor = Color.red;
    }

    public override void Initialize(BoidSettings settings) {
        base.Initialize(settings);
        BoidMan2D boidManager = GameObject.FindGameObjectWithTag("BoidManager").GetComponent<BoidMan2D>();
    }

    protected override void OnEnable() {
        base.OnEnable();
        allSharks.Add(this);
    }

    protected override void OnDisable() {
        base.OnDisable();
        allSharks.Remove(this);
    }

    void OnSawFish(FishBoid2D seenFish) {
        if (seenFish == null && targetFish == null) {
            return;
        }
        if (seenFish && targetFish) {
            lastKnownTargetPos = seenFish.transform.position;
            return;
        }
        if (seenFish != null && targetFish == null) {
            targetFish = seenFish;
            BeginChasing();
        }
        if (seenFish == null && targetFish != null) {
            loseFishTimerDelta -= Time.deltaTime;
            if (loseFishTimerDelta <= 0) {
                targetFish = null;
                loseFishTimerDelta = loseFishTimerMax;
                ReturnToIdle();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (IsInLayerMask(other.gameObject.layer, fishMask)) {
            other.GetComponent<AgentReproduction>().OnDeathAttempt.Invoke();
        }
    }
    void FixedUpdate() {
        Detect(110f, 5f, fishMask, onSawFish);
    }

    void BeginChasing() {
        state = SharkState.chasing;
        maxSpeed *= 1.5f;
    }

    void ReturnToIdle() {
        state = SharkState.idle;
        maxSpeed /= 1.5f;
    }

    protected override Vector2 ApplyCustomRules(Vector2 a) {
        a = base.ApplyCustomRules(a);
        if (state == SharkState.chasing) {
            a += SteerTowards(lastKnownTargetPos) * fishTargetWeight;
        }
        return a;
    }
}