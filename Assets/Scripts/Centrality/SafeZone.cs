using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SafeZone : MonoBehaviour
{
    [SerializeField] float deleteTimerMax;
    public float deleteTimer { get; private set; }

    public UnityEvent OnSafeZoneDelete = new();
    Collider safeCollider;

    void Start() {
        deleteTimer = deleteTimerMax;
        OnSafeZoneDelete.AddListener((DeleteAgentsOutsideZone));
    }

    void Update() {
        deleteTimer -= Time.deltaTime;
        deleteTimer = Math.Max(0f, deleteTimer);
        if (deleteTimer <= 0f) {
            OnSafeZoneDelete.Invoke();
            deleteTimer = deleteTimerMax;
        }
    }

    void DeleteAgentsOutsideZone() {
        foreach (Boid2D b in Boid2D.allBoids.ToList()) {
            float distance = Vector3.Distance(b.transform.position, transform.position);
            if (distance >= transform.localScale.x/2) {
                b.GetComponent<AgentReproduction>().OnDeathAttempt.Invoke();
            }
        }

    }
}
