using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentColorManager : MonoBehaviour
{

    Color _fishMatDefault = new Color(172, 252, 255);
    Color _fishTrailDefault = new Color(0, 168, 255);

    Color _prevMatColor;
    Color _prevTrailColor;

    Color _defaultMatColor;
    Color _defaultTrailColor;

    Color _flashMatColor;
    Color _flashTrailColor;

    MeshRenderer _mesh;
    TrailRenderer _trail;

    bool colChangeActive = false;
    float colChangeTimerMax;
    float colChangeTimer;

    void Start() {
        _mesh = GetComponentInChildren<MeshRenderer>();
        _trail = GetComponent<TrailRenderer>();
        _defaultMatColor = _mesh.material.color;
        _defaultTrailColor = _trail.startColor;
    }

    void Update() {
        if (!colChangeActive) return;
        colChangeTimer -= Time.deltaTime;
        Color lerpColor = Color.Lerp(_defaultMatColor, _flashMatColor, colChangeTimer / colChangeTimerMax);
        ChangeColor(lerpColor, lerpColor);
        if (colChangeTimer <= 0) {
            ResetColors();
            colChangeActive = false;
        }
    }

    public void ChangeColor(Color newMatColor, Color newTrailColor) {
        _prevMatColor = _mesh.material.color;
        _prevTrailColor = _trail.startColor;
        _mesh.material.color = newMatColor;
        _trail.startColor = newTrailColor;
    }

    public void ResetColors() {
        _mesh.material.color = _prevMatColor;
        _trail.startColor = _prevTrailColor;
    }

    public void FlashColorForSeconds(Color color, float duration) {
        colChangeTimer = colChangeTimerMax = duration;
        colChangeActive = true;
        _flashMatColor = _flashTrailColor = color;
    }
}
