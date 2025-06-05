// UnityWebSocketClient.cs
// Add this script to your Unity project for Dashboard integration

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;

public class UnityWebSocketClient : MonoBehaviour
{
    [Header("WebSocket Configuration")]
    public string serverUrl = "ws://localhost:8000/ws/unity";
    public float reconnectDelay = 3f;
    
    [Header("Building Components")]
    public GameObject electricitySystem;
    public GameObject solarPanels;
    public GameObject waterSystem;
    public GameObject hvacSystem;
    
    private WebSocket ws;
    private bool isConnected = false;
    private Dictionary<string, GameObject> componentMap;
    
    // Events for other scripts to listen to
    public static event Action<string, object> OnDataReceived;
    public static event Action<bool> OnConnectionChanged;
    
    void Start()
    {
        InitializeComponentMap();
        ConnectToWebSocket();
    }
    
    void InitializeComponentMap()
    {
        componentMap = new Dictionary<string, GameObject>
        {
            {"electricity_system", electricitySystem},
            {"solar_panels", solarPanels},
            {"water_system", waterSystem},
            {"hvac_system", hvacSystem}
        };
    }
    
    void ConnectToWebSocket()
    {
        try
        {
            ws = new WebSocket(serverUrl);
            
            ws.OnOpen += (sender, e) =>
            {
                Debug.Log("WebSocket connected to dashboard bridge");
                isConnected = true;
                OnConnectionChanged?.Invoke(true);
                
                // Request initial sync
                SendMessage(new
                {
                    type = "sync_request"
                });
            };
            
            ws.OnMessage += (sender, e) =>
            {
                try 
                {
                    var message = JsonConvert.DeserializeObject<WebSocketMessage>(e.Data);
                    HandleMessage(message);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing WebSocket message: {ex.Message}");
                }
            };
            
            ws.OnClose += (sender, e) =>
            {
                Debug.Log("WebSocket disconnected from dashboard bridge");
                isConnected = false;
                OnConnectionChanged?.Invoke(false);
                
                // Attempt to reconnect
                StartCoroutine(ReconnectAfterDelay());
            };
            
            ws.OnError += (sender, e) =>
            {
                Debug.LogError($"WebSocket error: {e.Message}");
                OnConnectionChanged?.Invoke(false);
            };
            
            ws.Connect();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize WebSocket: {ex.Message}");
            StartCoroutine(ReconnectAfterDelay());
        }
    }
    
    IEnumerator ReconnectAfterDelay()
    {
        yield return new WaitForSeconds(reconnectDelay);
        ConnectToWebSocket();
    }
    
    void HandleMessage(WebSocketMessage message)
    {
        Debug.Log($"Received message type: {message.type}");
        
        switch (message.type)
        {
            case "highlight_component":
                HighlightComponent(message.component, message.data);
                break;
                
            case "sensor_update":
                UpdateSensorVisualization(message.sensor, message.value);
                break;
                
            case "alert":
                ShowAlert(message.severity, message.message, message.component);
                break;
                
            case "sync_response":
                SyncWithDashboard(message.state);
                break;
        }
        
        // Notify other scripts
        OnDataReceived?.Invoke(message.type, message);
    }
    
    void HighlightComponent(string componentName, object data)
    {
        Debug.Log($"Dashboard clicked on: {componentName}");
        
        if (componentMap.ContainsKey(componentName))
        {
            GameObject component = componentMap[componentName];
            if (component != null)
            {
                StartCoroutine(HighlightEffect(component));
            }
        }
    }
    
    IEnumerator HighlightEffect(GameObject target)
    {
        // Get the renderer to change color
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer == null)
            renderer = target.GetComponentInChildren<Renderer>();
            
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            Color highlightColor = Color.yellow;
            
            // Animate highlight effect
            float duration = 2f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                float t = Mathf.PingPong(elapsed * 4, 1);
                renderer.material.color = Color.Lerp(originalColor, highlightColor, t);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            // Restore original color
            renderer.material.color = originalColor;
        }
        
        // Also animate scale for additional emphasis
        Vector3 originalScale = target.transform.localScale;
        LeanTween.scale(target, originalScale * 1.1f, 0.5f)
                 .setEase(LeanTweenType.easeOutBounce)
                 .setOnComplete(() => {
                     LeanTween.scale(target, originalScale, 0.5f);
                 });
    }
    
    void UpdateSensorVisualization(string sensor, float value)
    {
        // Update 3D model based on sensor data
        // Example: Change building lighting based on electricity usage
        
        switch (sensor)
        {
            case "electricity":
                UpdateElectricityVisualization(value);
                break;
            case "solar":
                UpdateSolarVisualization(value);
                break;
            case "water":
                UpdateWaterVisualization(value);
                break;
            case "temperature":
                UpdateTemperatureVisualization(value);
                break;
        }
    }
    
    void UpdateElectricityVisualization(float powerValue)
    {
        // Example: Change building lights intensity based on power usage
        Light[] buildingLights = electricitySystem.GetComponentsInChildren<Light>();
        float intensity = Mathf.Clamp01(powerValue / 50f); // Assuming max 50kW
        
        foreach (Light light in buildingLights)
        {
            light.intensity = intensity;
        }
    }
    
    void UpdateSolarVisualization(float solarValue)
    {
        // Example: Rotate solar panels based on generation
        if (solarPanels != null)
        {
            float rotationSpeed = solarValue * 2f; // Adjust multiplier as needed
            solarPanels.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
    
    void UpdateWaterVisualization(float flowRate)
    {
        // Example: Animate water particles based on flow rate
        ParticleSystem waterParticles = waterSystem.GetComponentInChildren<ParticleSystem>();
        if (waterParticles != null)
        {
            var emission = waterParticles.emission;
            emission.rateOverTime = flowRate;
        }
    }
    
    void UpdateTemperatureVisualization(float temperature)
    {
        // Example: Change building color based on temperature
        Renderer buildingRenderer = hvacSystem.GetComponent<Renderer>();
        if (buildingRenderer != null)
        {
            // Blue for cold, red for hot
            Color tempColor = Color.Lerp(Color.blue, Color.red, (temperature - 15f) / 20f);
            buildingRenderer.material.color = tempColor;
        }
    }
    
    void ShowAlert(string severity, string message, string component)
    {
        Debug.Log($"ALERT [{severity}] {component}: {message}");
        
        // You could create UI panels or visual indicators here
        // For now, we'll just highlight the component with alert color
        if (componentMap.ContainsKey(component))
        {
            StartCoroutine(AlertEffect(componentMap[component], severity));
        }
    }
    
    IEnumerator AlertEffect(GameObject target, string severity)
    {
        Color alertColor = severity == "error" ? Color.red : 
                          severity == "warning" ? Color.yellow : Color.green;
        
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            
            // Flash effect
            for (int i = 0; i < 6; i++)
            {
                renderer.material.color = alertColor;
                yield return new WaitForSeconds(0.2f);
                renderer.material.color = originalColor;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
    
    void SyncWithDashboard(object state)
    {
        Debug.Log("Syncing with dashboard state");
        // Handle initial sync data from dashboard
    }
    
    // Call this when user clicks on 3D building components
    public void OnBuildingComponentClicked(string componentName)
    {
        Debug.Log($"User clicked on Unity component: {componentName}");
        
        SendMessage(new
        {
            type = "unity_click",
            component = componentName,
            data = new
            {
                position = transform.position,
                timestamp = DateTime.Now.ToString("o")
            }
        });
    }
    
    void SendMessage(object message)
    {
        if (ws != null && ws.ReadyState == WebSocketState.Open)
        {
            string json = JsonConvert.SerializeObject(message);
            ws.Send(json);
        }
        else
        {
            Debug.LogWarning("WebSocket not connected, cannot send message");
        }
    }
    
    void OnDestroy()
    {
        if (ws != null)
        {
            ws.Close();
        }
    }
    
    // Make components clickable by adding this to building objects
    void OnMouseDown()
    {
        // Get component name from GameObject name or tag
        string componentName = gameObject.name.ToLower().Replace(" ", "_");
        OnBuildingComponentClicked(componentName);
    }
}

// Data structure for WebSocket messages
[Serializable]
public class WebSocketMessage
{
    public string type;
    public string component;
    public object data;
    public string sensor;
    public float value;
    public string severity;
    public string message;
    public object state;
    public string from;
    public string timestamp;
}