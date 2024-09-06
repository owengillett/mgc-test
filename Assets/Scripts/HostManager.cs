using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using UnityEngine.Networking;

public class HostManager : NetworkManager
{
    public System.Action OnPINCreated;

    public void TryHost()
    {
        StartCoroutine(HandleTryHost());
    }

    private IEnumerator HandleTryHost()
    {
        UnityWebRequest request = UnityWebRequest.Post(appURL + "/generate", "");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching Game PIN: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            GamePINData response = JsonUtility.FromJson<GamePINData>(jsonResponse);

            gamePIN = response.gamePIN;
            OnPINCreated?.Invoke();

            Debug.Log("Fetched new Game PIN: " + response.gamePIN);
            yield return StartCoroutine(StartWebSocket());
        }
    }

    protected override void OnWebSocketOpen()
    {
        base.OnWebSocketOpen();
        StartCoroutine(MaintainConnection());
    }

    private IEnumerator MaintainConnection ()
    {
        while (websocket.State == WebSocketState.Open)
        {
            Debug.Log("ping");
            NetworkMessage messageObject = new NetworkMessage
            {
                header = "ping"
            };

            yield return websocket.SendText(JsonUtility.ToJson(messageObject));
            yield return new WaitForSeconds(5f);
        }
    }

    protected override void OnWebSocketMessage(byte[] bytes)
    {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        string header = JsonUtility.FromJson<NetworkMessage>(message).header;

        //Debug.Log(header);

        switch (header)
        {
            case "cursor":

                Debug.Log(message);

                GameCursorInputData cursorData = JsonUtility.FromJson<NetworkCursorMessage>(message).data;
                GameCursorManager.Instance.HandleCursorInputData(cursorData);
                break;

            case "orientation":

                OrientationMessageData orientationData = JsonUtility.FromJson<OrientationMessageData>(message);
                GameCursorManager.Instance.HandleOrientationMessageData(orientationData);
                break;
        }

        base.OnWebSocketMessage(bytes);
    }
}

[System.Serializable]
public class GamePINData
{
    public string header;
    public string gamePIN;
}