using TMPro;
using UnityEngine;

public class GraphMarkerManager : MonoBehaviour
{
    [SerializeField] CustomGraph graph;
    [SerializeField] GameObject topMarker;
    [SerializeField] GameObject currValMarker;

    void Start() {
        graph.OnScaleChanged.AddListener(UpdateMarkers);
        graph.OnNewPlot.AddListener(UpdateMarkers);
    }

    void UpdateMarkers() {
        ScaleMarkerText();
        PlaceCurrValMarker();
        ChangeCurrValText();
    }

    void ChangeCurrValText() {
        TextMeshPro markerTMP = currValMarker.GetComponentInChildren<TextMeshPro>();
        markerTMP.text = graph.lastDataY.ToString("F2");
    }

    void PlaceCurrValMarker() {
        Vector3 pos = currValMarker.transform.position;
        LineRenderer lr = graph.boidLRs[0];
        pos.y = lr.GetPosition(lr.positionCount - 1).y;
        currValMarker.transform.position = pos;
    }

    void ScaleMarkerText() {
        float yScale = graph.GetGraphScale().y;
        TextMeshPro markerTextMesh = topMarker.GetComponentInChildren<TextMeshPro>();
        float scaledMarkerValue = YScaleToTopBound(yScale);
        markerTextMesh.text = scaledMarkerValue.ToString("F2");
    }

    // Calculates the top marker value, given current scale
    float YScaleToTopBound(float yScale) {
        float offset = topMarker.transform.position.y - graph.transform.position.y;
        return offset / yScale;
    }
}
