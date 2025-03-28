using UnityEngine;

public class AgentColorManager : MonoBehaviour
{
    Color _fishMatDefault = new Color(172, 252, 255);
    Color _fishTrailDefault = new Color(0, 168, 255);

    Color _prevMatColor;
    Color _prevTrailColor;

    MeshRenderer _mesh;
    TrailRenderer _trail;

    void Start() {
        _mesh = GetComponentInChildren<MeshRenderer>();
        _trail = GetComponent<TrailRenderer>();
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
}
