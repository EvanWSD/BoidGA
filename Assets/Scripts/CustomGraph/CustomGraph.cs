using System.Collections.Generic;
using UnityEngine;

enum GraphUtil {
    None,
    Time
}

public class CustomGraph : MonoBehaviour
{
    GraphFuncLib graphFuncLib;
    [SerializeField] Camera mainCam;
    [SerializeField] LineRenderer fishLR;
    [SerializeField] LineRenderer sharkLR;

    [SerializeField] [Range(0.01f, 1f)] float yScale = 0.5f;
    float oldYScale;
    [SerializeField] [Range(0.01f, 1f)] float xScale = 0.5f;
    float oldXScale;

    float timeToMax = 42f;
    float autoScaleDelta = 1.1f;

    // get values
    List<LineRenderer> boidLRs;

    float xG = 0f;
    float yG = 0f;

    int plotIndex = 0;
    bool graphStarted = false;
    float baseTime;

    float plotCDTimer;
    float plotCDTimerMax = 1f;

    List<Color> cols;

    void Start() {
        graphFuncLib = GetComponent<GraphFuncLib>();
        mainCam = Camera.main;
        cols = new List<Color>() { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue };
        oldYScale = yScale;
        oldXScale = xScale;
    }

    void StartGraph()
    {
        baseTime = Time.time;
        graphStarted = true;
        GetInitInfo();
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
                RescaleGraph();
            }

            plotCDTimer -= Time.deltaTime;
            if (plotCDTimer <= 0) {
                plotCDTimer = plotCDTimerMax;
                PlotPoints();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
            StartGraph();
    }

    // main function mapping X->Y (edit this!)
    float F(Transform t) {
        if (t.CompareTag("FishLine")) {
            return graphFuncLib.AverageGeneValue(FishBoid2D.allFish, "fovAngle");
        }

        if (t.CompareTag("SharkLine")) {
            return graphFuncLib.AverageGeneValue(SharkBoid2D.allSharks, "fovAngle");
        }
        return 0f;
    }

    void PlotPoints()
    {
        xG = Time.time - baseTime;
        foreach (LineRenderer boidLR in boidLRs)
        {
            boidLR.positionCount = plotIndex + 1;
            boidLR.SetPosition(plotIndex, new Vector3(
                xG * xScale,
                F(boidLR.transform) * yScale,
                0f
                ) + transform.position);
        }
        plotIndex++;
    }

    void RescaleGraph() {
        Vector3 transformPos = transform.position;
        foreach (LineRenderer lr in boidLRs) {
            for (int i = 0; i < lr.positionCount; i++) {
                Vector3 pos = lr.GetPosition(i);
                pos -= transformPos; // remove offset
                pos.y = pos.y * yScale / oldYScale;
                pos.x = pos.x * xScale / oldXScale;
                pos += transformPos; // reapply offset
                lr.SetPosition(i, pos);
            }
        }
        oldYScale = yScale;
        oldXScale = xScale;
    }

    void GetInitInfo() {
        boidLRs = new List<LineRenderer>() { fishLR, sharkLR };
    }


}
