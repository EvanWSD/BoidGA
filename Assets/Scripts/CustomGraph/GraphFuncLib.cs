using UnityEngine;

// Specialised per-project, for saving anything used for F(x) in CustomGraph.cs

    public class GraphFuncLib : MonoBehaviour
    {
        public GeneSelector geneSelector { get; private set; }
        CustomGraph graph;

        void Start() { // To be refactored, spaghetti
            graph = GetComponent<CustomGraph>();
            geneSelector = GetComponent<GeneSelector>();
            geneSelector.OnSelectGene.AddListener(() => {
                graph.SwitchGraph(geneSelector.selectedGene);
            });
        }
    }
