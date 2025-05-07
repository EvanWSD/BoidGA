using UnityEngine;

public class CentralBoid : Boid2D
{
    [SerializeField] GameObject safeZone;
    public float centralityWeight = 1f;

    protected override Vector2 ApplyCustomRules(Vector2 a) {
        a = base.ApplyCustomRules(a);
        a += (Vector2) (safeZone.transform.position - transform.position) * centralityWeight;
        return a;
    }
}
