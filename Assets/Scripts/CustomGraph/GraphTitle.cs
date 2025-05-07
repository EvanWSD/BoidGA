using TMPro;
using UnityEngine;

public class GraphTitle : MonoBehaviour
{
    [SerializeField] TextMeshPro titleTMP;
    GeneSelector geneSelector;
    CustomGraph graph;

    void Start() {
        GetComponent<CustomGraph>().OnGraphStart.AddListener(UpdateTitle);
        geneSelector = GetComponent<GeneSelector>();
        geneSelector.OnSelectGene.AddListener(UpdateTitle);
    }

    void UpdateTitle() {
        titleTMP.text = geneSelector.selectedGene;
    }
}
