using UnityEngine;
using UnityEngine.Events;

public class Boid2D : MonoBehaviour
{
    protected BoidSettings settings;
    protected float maxSpeed;
    Vector2 velocity;

    public Vector2 pos;
    public Vector2 headingDir;

    protected BoidMan2D boidManager;

    [HideInInspector] public Vector2 avgFlockHeading;
    [HideInInspector] public Vector2 avgAvoidanceHeading;
    [HideInInspector] public Vector2 centreOfFlockmates;
    [HideInInspector] public int numPerceivedFlockmates;

    protected Color debugColor = Color.white;

    public void Initialize(BoidSettings settings) {
        this.settings = settings;
        this.maxSpeed = settings.maxSpeed;
        float randomSpeed = Random.Range(settings.minSpeed, settings.maxSpeed);
        velocity = Random.insideUnitCircle * randomSpeed;
        pos = transform.position;
        headingDir = transform.up;
        boidManager = GameObject.FindGameObjectWithTag("BoidManager").GetComponent<BoidMan2D>();
    }

    public void UpdateBoid() {
        Vector2 acceleration = Vector2.zero;

        if (numPerceivedFlockmates != 0) {
            centreOfFlockmates /= numPerceivedFlockmates;
            Vector2 offsetToFlockmatesCentre = (centreOfFlockmates - pos);
            acceleration += SteerTowards(avgFlockHeading) * settings.alignWeight;
            acceleration += SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
            acceleration += SteerTowards(avgAvoidanceHeading) * settings.separationWeight;
        }

        acceleration = ApplyCustomRules(acceleration);

        velocity += acceleration * Time.deltaTime;
        velocity = ClampSpeed(velocity, settings.minSpeed, settings.maxSpeed);

        transform.position = pos + velocity * Time.deltaTime;
        pos = transform.position;
        headingDir = transform.up = velocity;
    }

    protected virtual Vector2 ApplyCustomRules(Vector2 a) {
        //a += SteerTowards(ObstacleAvoidance()) * settings.avoidCollisionWeight;
        a += SteerTowards(CentralTendency()) * settings.CTWeight;
        return a;
    }

    Vector2 ObstacleAvoidance() {
        Vector2 avoidForce = Vector2.zero;
        float fovAngle = settings.fovAngle;
        float rayCount = settings.rayCount;
        float angleStep = fovAngle / (rayCount - 1);
        float startAngle = -fovAngle / 2;

        for (int i = 0; i < rayCount; i++) {
            float angle = startAngle + (angleStep * i);
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * velocity.normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, settings.collisionAvoidDst,
                settings.obstacleMask);
            // Debug.DrawRay(transform.position, rayDirection * settings.collisionAvoidDst, Color.red);

            if (hit.collider != null) {
                Vector2 hitNormal = hit.normal;
                avoidForce += hitNormal * (settings.collisionAvoidDst - hit.distance);
                break;
            }
        }

        return avoidForce;
    }

    protected void Detect<T>(float fov, float distance, LayerMask mask, UnityEvent<T> onHit, int numRays = -1)
        where T : MonoBehaviour {
        float rayCount = (numRays == -1) ? settings.rayCount : numRays;
        float angleStep = fov / (rayCount - 1);
        float startAngle = -fov / 2;

        for (int i = 0; i < rayCount; i++) {
            float angle = startAngle + (angleStep * i);
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * velocity.normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, distance,
                mask);
            //Debug.DrawRay(transform.position, rayDirection * settings.collisionAvoidDst, debugColor);

            if (hit.collider != null) {
                onHit.Invoke(hit.transform.GetComponent<T>());
                break;
            }

            onHit.Invoke(default); // no hits (null)
        }
    }

    protected Vector2 SteerTowards(Vector2 vector) {
        Vector2 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector2.ClampMagnitude(v, settings.maxTurnRate);
    }

    Vector2 ClampSpeed(Vector2 v, float minSpeed, float maxSpeed) {
        float spd = v.magnitude;
        Vector2 dir = v / spd;
        spd = Mathf.Clamp(spd, minSpeed, maxSpeed);
        return dir * spd;
    }

    Vector2 CentralTendency() {
        float sqrMag = pos.magnitude * pos.magnitude;
        if (pos.magnitude <= settings.CTIgnoreRadius) {
            return Vector3.zero;
        }

        return -pos.normalized * sqrMag;
    }

    protected bool IsInLayerMask(LayerMask collisionLayer, LayerMask targetMask) {
        return (((1 << collisionLayer) & targetMask) != 0);
    }
}