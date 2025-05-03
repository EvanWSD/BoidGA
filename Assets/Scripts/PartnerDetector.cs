using UnityEngine;
using UnityEngine.Events;

public class PartnerDetector : MonoBehaviour
{
    public bool available;
    public UnityEvent<AgentReproduction> OnPartnerFound = new();
    public LayerMask partnerMask;

    void OnTriggerEnter2D(Collider2D other) {
        if (available && other.transform.parent.TryGetComponent(out AgentReproduction otherAgent)) {
            OnPartnerFound.Invoke(otherAgent);
        }
    }

    bool IsOnLayer(GameObject obj, LayerMask mask) {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
