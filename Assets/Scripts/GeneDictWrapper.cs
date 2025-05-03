using System.Collections.Generic;

[System.Serializable]
public class GeneDictWrapper
{
    public List<string> _keys = new();
    public List<GeneData> _values = new();

    public GeneDictWrapper(Dictionary<string, GeneData> dict) {
        foreach(var kvp in dict) {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public Dictionary<string, GeneData> ToDict() {
        var result = new Dictionary<string, GeneData>();
        for (int i = 0; i < _keys.Count; i++) {
            result[_keys[i]] = _values[i];
        }
        return result;
    }
}