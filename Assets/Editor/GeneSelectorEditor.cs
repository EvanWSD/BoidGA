using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GeneSelector))]
    public class GeneSelectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            GeneSelector selector = (GeneSelector)target;

            // Show the options in a dropdown
            int currentIndex = Mathf.Max(0, selector.geneOptions.IndexOf(selector.selectedGene));
            currentIndex = EditorGUILayout.Popup("Selected Gene", currentIndex, selector.geneOptions.ToArray());

            // Save selection
            string newSelection = selector.selectedGene;
            try {
                newSelection = selector.geneOptions[currentIndex];
            } catch (System.ArgumentOutOfRangeException) {
                return;
            }

            if (newSelection != selector.selectedGene)
            {
                selector.selectedGene = newSelection;
                EditorUtility.SetDirty(selector);
            }
        }
    }
}