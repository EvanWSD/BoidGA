using UnityEngine;

public class AgentHunger : MonoBehaviour
{
    float _hunger = 20f;
    [SerializeField] float maxHunger = 20f;
    [SerializeField] float hungerRate = 0.1f;

    void Update() {
        _hunger -= hungerRate * Time.deltaTime;
        if (_hunger <= 0) {
            Die();
        }
    }

    public void Feed(float foodValue) {
        _hunger += foodValue;
        _hunger = Mathf.Max(_hunger, maxHunger);
    }

    void Die() {
        Destroy(gameObject);
        // more to do here, could stop moving and still be food
    }
}
