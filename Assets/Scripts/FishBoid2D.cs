using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum FishState
{
    idle,
    fleeing,
}

public class FishBoid2D : Boid2D
{
    public static List<Boid2D> allFish = new();

    [SerializeField] LayerMask sharkMask;
    public float fearWeight;
    FishState state = FishState.idle;
    public UnityEvent<SharkBoid2D> onSawShark;

    public bool posterBoid;

    const float SCARED_SPEED_MULT = 1.1f;
    float calmTimerMax = 0.3f;
    float calmTimerDelta = 0.3f;

    Vector3 toDetectedShark;

    void Start() {
        onSawShark.AddListener(OnSawShark);
        debugColor = Color.cyan;
    }
    protected override Vector2 ApplyCustomRules(Vector2 a) {
        a = base.ApplyCustomRules(a);
        if (state == FishState.fleeing) {
            a += SteerTowards(-toDetectedShark) * fearWeight;
        }
        return a;
    }

    public override void Initialize(BoidSettings settings) {
        base.Initialize(settings);
        calmTimerDelta = calmTimerMax;
    }

    protected override void OnEnable() {
        base.OnEnable();
        allFish.Add(this);
    }

    protected override void OnDisable() {
        base.OnDisable();
        allFish.Remove(this);
    }


    void OnSawShark(SharkBoid2D seenShark) {
        if (seenShark == null) {
            if (state == FishState.fleeing) {
                calmTimerDelta -= Time.deltaTime;
                if (calmTimerDelta <= 0) {
                    ReturnToIdle();
                    calmTimerDelta = calmTimerMax;
                }
            }
            return;
        }

        toDetectedShark = seenShark.transform.position - transform.position;
        if (state == FishState.idle) {
            BeginFleeing();
        }
    }

    void FixedUpdate() {
        Detect(270f, 5f, sharkMask, onSawShark, 15);
    }

    void BeginFleeing() {
        state = FishState.fleeing;
        maxSpeed *= SCARED_SPEED_MULT;
    }

    void ReturnToIdle() {
        state = FishState.idle;
        maxSpeed /= SCARED_SPEED_MULT;
    }


    #if UNITY_EDITOR
    void OnDrawGizmos() {
        if (posterBoid) {
            if (settings.visSeparation) PosterVisSeparation();
            if (settings.visAlignment) PosterVisAlignment();
            if (settings.visCohesion) PosterVisCohesion();
        }
    }
    #region Poster Visualisation
    void PosterVisSeparation() {
        Gizmos.color = Color.green;
        float radius = settings.separationRadius;
        int segments = 64;
        float angleStep = 360f / segments;
        Vector3 center = transform.position;
        Vector3 prevPoint = center + Vector3.right * radius;
        for (int i = 1; i <= segments; i++) {
            float rad = Mathf.Deg2Rad * angleStep * i;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
            DrawThickLine(prevPoint, newPoint, Color.green + Color.grey, 5f);
            prevPoint = newPoint;
        }
    }

    void PosterVisAlignment() {
        float radius = settings.perceptionRadius;
        foreach (FishBoid2D boid in allFish) {
            Transform boidTransform = boid.transform;
            DrawThickLine(boidTransform.position, boidTransform.position + boidTransform.up * 0.5f, Color.red, 5f);
            if (boid != this && Vector3.Distance(this.transform.position, boidTransform.position) <= radius) {
                DrawThickLine(transform.position, boid.transform.position, Color.yellow + Color.red + Color.red, 3f);
            }
        }
    }

    void PosterVisCohesion() {
        float radius = settings.perceptionRadius;
        foreach (FishBoid2D otherBoid in allFish) {
            Vector3 toOtherBoid = otherBoid.transform.position - transform.position;
            if (otherBoid != this && toOtherBoid.magnitude <= radius) {
                DrawThickLine(otherBoid.transform.position, centreOfFlockmates, Color.white, 3);
            }
        }
    }

    void DrawThickLine(Vector3 startPosition, Vector3 endPosition, Color col, float thickness) {
        var p1 = startPosition;
        var p2 = endPosition; ;
        Handles.DrawBezier(p1,p2,p1,p2, col,null,thickness);
    }
    #endregion
    #endif
}
