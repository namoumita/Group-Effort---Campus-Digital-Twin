using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RoomDataFetcher : MonoBehaviour
{
    public string apiUrl = "http://127.0.0.1:5000/api/rooms";

    void Start()
    {
        StartCoroutine(FetchRoomData());
    }

    IEnumerator FetchRoomData()
    {
        while (true)
        {
            UnityWebRequest request = UnityWebRequest.Get(apiUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string wrappedJson = "{\"rooms\":" + request.downloadHandler.text + "}";
                RoomDataList dataList = JsonUtility.FromJson<RoomDataList>(wrappedJson);

                foreach (RoomData room in dataList.rooms)
                {
                    GameObject obj = GameObject.Find(room.roomId);
                    if (obj != null)
                    {
                        obj.GetComponent<RoomVisualizer>()?.UpdateRoom(room);
                    }
                    else
                    {
                        Debug.LogWarning("Room GameObject not found: " + room.roomId);
                    }
                }
            }
            else
            {
                Debug.LogError("API Error: " + request.error);
            }

            yield return new WaitForSeconds(5f); // Poll every 5 seconds
        }
    }
}
