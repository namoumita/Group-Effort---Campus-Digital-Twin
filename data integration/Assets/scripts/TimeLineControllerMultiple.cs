using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TimeLineControllerMultiple : MonoBehaviour
{
    public TMP_Dropdown yearDropdown, monthDropdown, dayDropdown, timeDropdown;

    public MeterBoard electricityMeter, gasMeter, waterMeter;
    public MultiCSVReader csvReader;

    Dictionary<int, Dictionary<int, Dictionary<int, List<DataPoint2>>>> groupedData = new();

    void Start()
    {
        // Organize combinedData by year/month/day
        foreach (var dp in csvReader.combinedData)
        {
            int y = dp.timestamp.Year, m = dp.timestamp.Month, d = dp.timestamp.Day;

            if (!groupedData.ContainsKey(y)) groupedData[y] = new();
            if (!groupedData[y].ContainsKey(m)) groupedData[y][m] = new();
            if (!groupedData[y][m].ContainsKey(d)) groupedData[y][m][d] = new();

            groupedData[y][m][d].Add(dp);
        }

        // Populate dropdowns...
        PopulateYearDropdown();
    }

    void PopulateYearDropdown()
    {
        yearDropdown.ClearOptions();
        var years = groupedData.Keys.OrderBy(y => y).ToList();
        yearDropdown.AddOptions(years.Select(y => y.ToString()).ToList());

        yearDropdown.onValueChanged.AddListener(_ => PopulateMonthDropdown());
        PopulateMonthDropdown();
    }

    void PopulateMonthDropdown()
    {
        int year = int.Parse(yearDropdown.options[yearDropdown.value].text);
        monthDropdown.ClearOptions();
        var months = groupedData[year].Keys.OrderBy(m => m).ToList();
        monthDropdown.AddOptions(months.Select(m => m.ToString()).ToList());

        monthDropdown.onValueChanged.AddListener(_ => PopulateDayDropdown());
        PopulateDayDropdown();
    }

    void PopulateDayDropdown()
    {
        int year = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int month = int.Parse(monthDropdown.options[monthDropdown.value].text);
        dayDropdown.ClearOptions();
        var days = groupedData[year][month].Keys.OrderBy(d => d).ToList();
        dayDropdown.AddOptions(days.Select(d => d.ToString()).ToList());

        dayDropdown.onValueChanged.AddListener(_ => PopulateTimeDropdown());
        PopulateTimeDropdown();
    }

    void PopulateTimeDropdown()
    {
        int year = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int month = int.Parse(monthDropdown.options[monthDropdown.value].text);
        int day = int.Parse(dayDropdown.options[dayDropdown.value].text);

        var points = groupedData[year][month][day];
        timeDropdown.ClearOptions();
        timeDropdown.AddOptions(points.Select(p => p.timestamp.ToString("HH:mm")).ToList());

        timeDropdown.onValueChanged.AddListener(OnTimeChanged);
        OnTimeChanged(0);
    }

    void OnTimeChanged(int index)
    {
        int year = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int month = int.Parse(monthDropdown.options[monthDropdown.value].text);
        int day = int.Parse(dayDropdown.options[dayDropdown.value].text);
        var points = groupedData[year][month][day];
        var dp = points[index];

        electricityMeter.UpdateConsumption(dp.electricity, dp.timestamp);
        gasMeter.UpdateConsumption(dp.gas, dp.timestamp);
        waterMeter.UpdateConsumption(dp.water, dp.timestamp);
    }
}
