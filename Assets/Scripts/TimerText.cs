using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.Util;

public class TimerText : MonoBehaviour
{
    TextMeshPro textMesh;
    float target;

    [SerializeField] SafeZone safeZone;

    // Change!
    float GetTarget() {
        return safeZone.deleteTimer;
    }

    void Start() {
        textMesh = GetComponent<TextMeshPro>();
    }

    void Update() {
        target = GetTarget();
        textMesh.text = target.ToString("00.00");
    }



}
