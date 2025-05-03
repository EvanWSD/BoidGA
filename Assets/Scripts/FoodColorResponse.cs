using UnityEngine;

public class FoodColorResponse : MonoBehaviour
{
    [SerializeField] Color responseColor = Color.green;
    [SerializeField] float duration = 2f;

    AgentColorManager colorManager;
    AgentHunger agentHunger;

    void Start() {
        colorManager = GetComponent<AgentColorManager>();
        agentHunger = GetComponent<AgentHunger>();
        agentHunger.OnEat.AddListener(() => {
            colorManager.FlashColorForSeconds(responseColor, duration);
        });
    }
}
