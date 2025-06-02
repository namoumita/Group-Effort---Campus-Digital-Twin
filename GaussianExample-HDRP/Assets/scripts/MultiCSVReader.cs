using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class DataPoint2
{
    public DateTime timestamp;
    public float electricity;
    public float gas;
    public float water;
}

public class MultiCSVReader : MonoBehaviour
{
   public TextAsset electricityCSV;
    public TextAsset gasCSV;
    public TextAsset waterCSV;

    public List<DataPoint2> combinedData = new();

    void Awake()
    {
        var electricity = ParseCSV(electricityCSV);
        var gas = ParseCSV(gasCSV);
        var water = ParseCSV(waterCSV);

        // Merge by timestamp
        foreach (var e in electricity)
        {
            var match = new DataPoint2 { timestamp = e.Key, electricity = e.Value };
            match.gas = gas.ContainsKey(e.Key) ? gas[e.Key] : 0f;
            match.water = water.ContainsKey(e.Key) ? water[e.Key] : 0f;

            combinedData.Add(match);
        }

        combinedData = combinedData.OrderBy(dp => dp.timestamp).ToList();
    }

    Dictionary<DateTime, float> ParseCSV(TextAsset csv)
    {
        var dict = new Dictionary<DateTime, float>();
        var lines = csv.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines.Skip(1)) // skip header
        {
            var parts = line.Split(',');
            if (DateTime.TryParse(parts[0], out DateTime time) &&
                float.TryParse(parts[1], out float val))
            {
                dict[time] = val;
            }
        }

        return dict;
    }
}
