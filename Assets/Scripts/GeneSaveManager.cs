using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GeneSaveManager
{
    public static void SaveGenes(Dictionary<string, GeneData> genesToSave, string fileName="genes.json") {
        GeneDictWrapper geneWrapper = new GeneDictWrapper(genesToSave);
        string json = JsonUtility.ToJson(geneWrapper, true);
        //string path = Application.persistentDataPath + fileName; // saves to AppData/Local on windows
        string directory = Path.Combine(Application.dataPath, "Saves"); // saves to Assets/Saves (in editor)
        string filePath = Path.Combine(directory, fileName);
        File.WriteAllText(filePath, json);
    }

    public static Dictionary<string, GeneData> LoadGenes(string fileName="genes.json") {
        string directory = Path.Combine(Application.dataPath, "Saves");
        string filePath = Path.Combine(directory, fileName);
        if (!File.Exists(filePath)) {
            Debug.LogError("Gene save file not found!");
            return null;
        }

        string json = File.ReadAllText(filePath);
        GeneDictWrapper geneWrapper = JsonUtility.FromJson<GeneDictWrapper>(json);
        return geneWrapper.ToDict();
    }
}