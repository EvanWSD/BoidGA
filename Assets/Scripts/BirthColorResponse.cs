using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthColorResponse : MonoBehaviour
{
    [SerializeField] Color responseColor = Color.white;
    [SerializeField] float duration = 3f;


    AgentColorManager colorManager;
    AgentReproduction agentReproduction;

    void Start() {
        colorManager = GetComponent<AgentColorManager>();
        agentReproduction = GetComponent<AgentReproduction>();
        agentReproduction.OnReproduce.AddListener(() => {
            colorManager.FlashColorForSeconds(responseColor, duration);
        });
    }
}
