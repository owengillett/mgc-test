using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using UnityEngine.Networking;

public class ClientManager : NetworkManager
{
    public System.Action OnPINReject;

    public void TryConnect(string PIN)
    {
        StartCoroutine(HandleTryConnect(PIN));
    }

    private IEnumerator HandleTryConnect(string PIN)
    {
        PIN = CleanString(PIN);

        string statusUrl = appURL + "/status/" + PIN;
        UnityWebRequest uwr = UnityWebRequest.Get(statusUrl);

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Network error while validating PIN: " + uwr.error);
        }
        else
        {
            GamePINValidation response = JsonUtility.FromJson<GamePINValidation>(uwr.downloadHandler.text);

            Debug.Log(response.header + response.isValid);

            if (response.header == "status" && response.isValid)
            {
                Debug.Log("PIN is valid! Starting WebSocket...");
                gamePIN = PIN;
                yield return StartCoroutine(StartWebSocket());
            }
            else
            {
                Debug.Log("Invalid PIN");
            }
        }
    }

    protected override void Update()
    {
        base.Update();
  
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            var cursorData = new GameCursorInputData()
            {
                id = "client",
                isPointerDown = Input.GetMouseButton(0),
                screenPos = Input.mousePosition,
                rotation = 0
            };

            NetworkCursorMessage message = new NetworkCursorMessage
            {
                header = "cursor",
                data = cursorData
            };

            string jsonMessage = JsonUtility.ToJson(message);
            websocket.SendText(jsonMessage);
        }
    }
}

[System.Serializable]
public class GamePINValidation
{
    public string header;
    public bool isValid;
}