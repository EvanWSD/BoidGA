using UnityEngine;

public class AgentColorManager : MonoBehaviour
{
    Color _fishMatDefault = new Color(172, 252, 255);
    Color _fishTrailDefault = new Color(0, 168, 255);

    MeshRenderer _mesh;
    TrailRenderer _trail;

    void Start() {
        _mesh = GetComponentInChildren<MeshRenderer>();
        _trail = GetComponent<TrailRenderer>();
    }

    public void ChangeColor(Color newMatColor, Color newTrailColor) {
        _mesh.material.color = newMatColor;
        _trail.startColor = newTrailColor;
    }
}
