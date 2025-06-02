using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MeterBoard : MonoBehaviour
{

    public TextMeshPro text3D;
    public string unit;
    public void UpdateConsumption(float value, System.DateTime time)
    {
        text3D.text = $"Time: {time:HH:mm}\nUsage: {value:0.0000} {unit}";
    }


}
