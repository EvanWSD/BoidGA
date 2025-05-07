using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpeciesManager))]
public class SpeciesManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SpeciesManager speciesManager = (SpeciesManager)target;

        if (speciesManager.allSpecies == null || speciesManager.allSpecies.Count == 0)
        {
            EditorGUILayout.HelpBox("No species available.", MessageType.Warning);
            return;
        }

        // Get index of currently selected species
        int currentIndex = Mathf.Max(0, speciesManager.allSpecies.IndexOf(speciesManager.selectedSpecies));

        // Create a popup list with species names
        List<string> speciesNames = speciesManager.allSpecies.ConvertAll(s => s.name);
        int newIndex = EditorGUILayout.Popup("Selected species", currentIndex, speciesNames.ToArray());

        // Set the selectedSpecies based on dropdown
        Species newSelection = speciesManager.allSpecies[newIndex];

        if (newSelection != speciesManager.selectedSpecies)
        {
            speciesManager.selectedSpecies = newSelection;
            EditorUtility.SetDirty(speciesManager); // Ensure Unity saves the change
        }
    }
}