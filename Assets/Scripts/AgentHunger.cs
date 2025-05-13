using UnityEngine;
using UnityEngine.Events;

public class AgentHunger : MonoBehaviour
{
    float _hunger = 20f;
    [SerializeField] float maxHunger = 20f;
    [SerializeField] float hungerRate = 0.1f;

    public UnityEvent OnEat = new();

    void Update() {
        _hunger -= hungerRate * Time.deltaTime;
        if (_hunger <= 0) {
            GetComponent<AgentReproduction>().OnDeathAttempt.Invoke();
            _hunger = maxHunger; // won't trigger if successful death
        }
    }

    public void Feed(float foodValue) {
        _hunger += foodValue;
        _hunger = Mathf.Max(_hunger, maxHunger);
        OnEat.Invoke();
    }

    void Die() {

        Destroy(gameObject);
    }
}
