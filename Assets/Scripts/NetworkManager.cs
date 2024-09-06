using UnityEngine;
using System.Linq;
using NativeWebSocket;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public WebSocket websocket;
    public string gamePIN;

    public static string appURL = "https://mcgc-server-59dd53227764.herokuapp.com";
    public static string wsURL = "wss://mcgc-server-59dd53227764.herokuapp.com";

    public IEnumerator StartWebSocket ()
    {
        websocket = new WebSocket(wsURL);

        websocket.OnOpen += () =>
        {
            Debug.Log("WebSocket connection open!");
            OnWebSocketOpen();
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("WebSocket error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("WebSocket connection closed!");
            OnWebSocketClose(e);
        };

        websocket.OnMessage += (bytes) =>
        {
            OnWebSocketMessage(bytes);
        };

        var connectTask = websocket.Connect();
        yield return new WaitUntil(() => connectTask.IsCompleted);
    }

    protected virtual void OnWebSocketOpen ()
    {
        NetworkMessage messageObject = new NetworkMessage
        {
            header = "pair",
            body = gamePIN
        };

        websocket.SendText(JsonUtility.ToJson(messageObject));
    }

    protected virtual void OnWebSocketClose (WebSocketCloseCode e) { }
    protected virtual void OnWebSocketMessage(byte[] bytes) { }

    protected virtual void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }

    protected virtual async void OnApplicationQuit()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }

    public static string CleanString(string x)
    {
        return new string(x.Where(c => char.IsLetterOrDigit(c)).ToArray());
    }
}

[System.Serializable]
public class NetworkMessage
{
    public string header;
    public string body;
}

[System.Serializable]
public class NetworkCursorMessage
{
    public string header;
    public GameCursorInputData data;
}
