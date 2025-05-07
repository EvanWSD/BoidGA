using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphTimer : MonoBehaviour
{
    [SerializeField] TextMeshPro timerText;
    CustomGraph graph;

    void Start() {
        graph = GetComponent<CustomGraph>();
    }

    void Update() {
        timerText.text = FormatTime(graph.GetGraphTimeElapsed());
    }

    string FormatTime(float time)
    {
        int mins = Mathf.FloorToInt(time / 60f);
        int secs = Mathf.FloorToInt(time % 60f);
        int hundredths = Mathf.FloorToInt((time * 100f) % 100f);
        return $"{mins:00}:{secs:00}.{hundredths:00}";
    }

}
