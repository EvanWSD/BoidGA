using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

enum GraphUtil {
    None,
    Time
}

public class CustomGraph : MonoBehaviour
{
    GraphFuncLib graphFuncLib;
    [SerializeField] Camera mainCam;

    public UnityEvent OnScaleChanged { get; } = new();
    [SerializeField] [Range(0.01f, 10f)] float yScale = 0.5f;
    float oldYScale;
    [SerializeField] [Range(0.01f, 10f)] float xScale = 0.5f;
    float oldXScale;

    float timeToMax = 35f;
    float autoScaleDelta = 1.1f;

    // get values
    [SerializeField] public List<LineRenderer> boidLRs;

    float xG = 0f;
    float yG = 0f;
    public float lastDataX { get; private set; }
    public float lastDataY { get; private set; }

    int plotIndex = 0;
    bool graphStarted = false;
    public UnityEvent OnGraphStart { get; } = new();
    float baseTime;

    float plotCDTimer;
    float plotCDTimerMax = 1f;
    public UnityEvent OnNewPlot { get; } = new();

    List<Color> cols;

    List<Dictionary<string, GeneData>> graphData = new();

    void Start() {
        graphFuncLib = GetComponent<GraphFuncLib>();
        mainCam = Camera.main;
        cols = new List<Color>() { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue };
        oldYScale = yScale;
        oldXScale = xScale;

        OnGraphStart.AddListener(() => {
            InitGraphValues();
            OnScaleChanged.Invoke();
        });
        OnGraphStart.Invoke();
    }

    void InitGraphValues() {
        baseTime = Time.time;
        graphStarted = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            mainCam.enabled = !mainCam.enabled;
        }


        if (graphStarted)
        {
            if (Time.time - baseTime > timeToMax) {
                xScale /= autoScaleDelta;
                timeToMax *= autoScaleDelta;
            }

            if (oldYScale != yScale || oldXScale != xScale) {
                OnScaleChanged.Invoke();
                RescaleGraph();
            }

            plotCDTimer -= Time.deltaTime;
            if (plotCDTimer <= 0) {
                plotCDTimer = plotCDTimerMax;
                CaptureGraphData();
                PlotNewPoint();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
            OnGraphStart.Invoke();
    }

    // main function mapping X->Y (edit this!)
    float F(Transform t) {
        float v = graphData[^1][graphFuncLib.geneSelector.selectedGene].value;
        //Debug.Log(v);
        return v;
    }

    void PlotNewPoint()
    {
        xG = Time.time - baseTime;
        lastDataX = xG;
        foreach (LineRenderer boidLR in boidLRs) // per line
        {
            boidLR.positionCount = plotIndex + 1;
            float output = lastDataY = F(boidLR.transform);

            boidLR.SetPosition(plotIndex, new Vector3(
                xG * xScale,
                output * yScale,
                0f
                ) + transform.position);
        }
        plotIndex++;
        OnNewPlot.Invoke();
    }

    void RescaleGraph() {
        Vector3 transformPos = transform.position;
        foreach (LineRenderer lr in boidLRs) {
            for (int i = 0; i < lr.positionCount; i++) {
                Vector3 pos = lr.GetPosition(i);
                pos -= transformPos; // LineRenderers use world space by default!!
                pos.y = pos.y * yScale / oldYScale;
                pos.x = pos.x * xScale / oldXScale;
                pos += transformPos;
                lr.SetPosition(i, pos);
            }
        }
        oldYScale = yScale;
        oldXScale = xScale;
    }

    void CaptureGraphData() {
        graphData.Add(GeneManager.AvgGeneDict(SpeciesManager.Instance.selectedSpecies));
    }

    public void SwitchGraph(string selectedGene) {
        Vector3 transformPos = transform.position;
        foreach (LineRenderer speciesLine in boidLRs) {
            for (int i = 0; i < graphData.Count; i++) {
                Vector3 pos = speciesLine.GetPosition(i);
                pos -= transformPos;
                pos.y = graphData[i][selectedGene].value * yScale;
                pos += transformPos;
                speciesLine.SetPosition(i, pos);
            }
        }
    }

    public Vector2 GetGraphScale() {
        return new Vector2(xScale, yScale);
    }

    public float GetGraphTimeElapsed() {
        return Time.time - baseTime;
    }
}
