using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TwinUpdater : MonoBehaviour
{
    public string twinId = "Room101";  // Match your FastAPI twin ID
    public string backendUrl = "http://127.0.0.1:8000/mock-twin/unity/";

    public Renderer targetRenderer; // Assign a building part or the entire model

    void Start()
    {
        StartCoroutine(PollTwinData());
    }

    IEnumerator PollTwinData()
    {
        while (true)
        {
            UnityWebRequest request = UnityWebRequest.Get(backendUrl + twinId);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                TwinData twin = JsonUtility.FromJson<TwinData>(json);
                UpdateColorBasedOnLight(twin.lightLevel);
            }
            else
            {
                Debug.LogWarning("Error polling twin: " + request.error);
            }

            yield return new WaitForSeconds(5); // Repeat every 5 seconds
        }
    }

    void UpdateColorBasedOnLight(int lightLevel)
    {
        if (targetRenderer != null)
        {
            float intensity = Mathf.Clamp01(lightLevel / 100f);
            Color dynamicColor = new Color(1 - intensity, intensity, 0f); // From red to green
            targetRenderer.material.color = dynamicColor;
        }
    }
}

[System.Serializable]
public class TwinData
{
    public string id;
    public float temperature;
    public bool occupancy;
    public int lightLevel;
}
