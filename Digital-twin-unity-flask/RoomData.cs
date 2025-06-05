[System.Serializable]
public class RoomData
{
    public string roomId;
    public float temperature;
    public string electricity;
    public int occupancy;
}

[System.Serializable]
public class RoomDataList
{
    public RoomData[] rooms;
}
