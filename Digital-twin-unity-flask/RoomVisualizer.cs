using UnityEngine;

public class RoomVisualizer : MonoBehaviour
{
    public void UpdateRoom(RoomData data)
    {
        Debug.Log(
            $"Room: {data.roomId} | Temp: {data.temperature}°C | Electricity: {data.electricity} | Occupancy: {data.occupancy}"
        );
    }
}
