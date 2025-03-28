using System.Collections;
using UnityEngine;

public class FoodColorResponse : MonoBehaviour
{
    AgentColorManager colorManager;
    AgentHunger agentHunger;

    void Start() {
        colorManager = GetComponent<AgentColorManager>();
        agentHunger = GetComponent<AgentHunger>();
        agentHunger.OnEat.AddListener(() => {
            StartCoroutine(FlashGreenForSeconds(0.5f));
        });
    }

    IEnumerator FlashGreenForSeconds(float duration) {
        colorManager.ChangeColor(Color.green, Color.green);
        yield return new WaitForSeconds(duration);
        colorManager.ResetColors();
    }
}
