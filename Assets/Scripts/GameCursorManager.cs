using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursorManager : Singleton<GameCursorManager>
{
    public Dictionary<string, GameCursor> cursors = new Dictionary<string, GameCursor>();
    public GameCursor cursorPrefab;

    public System.Action OnPreCursorUpdate;

    private void Start()
    {
        Cursor.visible = false;
    }

    public void HandleOrientationMessageData(OrientationMessageData orientation)
    {
        //Debug.Log(JsonUtility.ToJson(orientation));

        bool facingUp = (Quaternion.Euler(0, 0, orientation.rotation.y) * Vector3.up).y > 0;

        float maxYawAngleDelta = 17.5f;
        float maxPitchAngleDelta = 17.5f * ((float)Screen.height / (float)Screen.width);

        float pitch = facingUp ? orientation.rotation.y : (180f - orientation.rotation.y);
        float yaw = -orientation.rotation.x + (facingUp ? 0 : 180f);

        float roll = -(orientation.rotation.z + (facingUp ? 0 : 180f));

        cursors.TryGetValue(orientation.id, out GameCursor cursor);

        if (cursor == null)
        {
            cursor = Instantiate(cursorPrefab);
            cursor.transform.SetParent(this.transform);
            cursor.Initialize(orientation.id);
            cursor.pitchCenterAngle = pitch;
            cursor.yawCenterAngle = yaw;
            cursors.Add(orientation.id, cursor);
        }

        var yawDelta = Mathf.DeltaAngle(cursor.yawCenterAngle, yaw);
        var pitchDelta = Mathf.DeltaAngle(cursor.pitchCenterAngle, pitch);

        // Adjust the center angles if the deltas exceed the max deltas
        if (yawDelta > maxYawAngleDelta)
        {
            cursor.yawCenterAngle += yawDelta - maxYawAngleDelta;
            yawDelta = maxYawAngleDelta;
        }
        else if (yawDelta < -maxYawAngleDelta)
        {
            cursor.yawCenterAngle += yawDelta + maxYawAngleDelta;
            yawDelta = -maxYawAngleDelta;
        }

        if (pitchDelta > maxPitchAngleDelta)
        {
            cursor.pitchCenterAngle += pitchDelta - maxPitchAngleDelta;
            pitchDelta = maxPitchAngleDelta;
        }
        else if (pitchDelta < -maxPitchAngleDelta)
        {
            cursor.pitchCenterAngle += pitchDelta + maxPitchAngleDelta;
            pitchDelta = -maxPitchAngleDelta;
        }

        float normalizedX = yawDelta / maxYawAngleDelta * 0.5f + 0.5f;

        Debug.Log(Screen.width + " " + Screen.height);

        float normalizedY = pitchDelta / maxPitchAngleDelta * 0.5f + 0.5f;

        cursor.targetScreenPos = new Vector2(normalizedX * Screen.width, normalizedY * Screen.height);
        cursor.targetRotation = roll;
        cursor.isPointerDown = orientation.isPointerDown;
    }

    public void HandleCursorInputData(GameCursorInputData inputData)
    {
        cursors.TryGetValue(inputData.id, out GameCursor cursor);

        if (cursor == null)
        {
            cursor = Instantiate(cursorPrefab);
            cursor.transform.SetParent(this.transform);
            cursor.Initialize(inputData.id);
            cursors.Add(inputData.id, cursor);
        }

        cursor.targetScreenPos = inputData.screenPos;
        cursor.targetRotation = inputData.rotation;
        cursor.isPointerDown = inputData.isPointerDown;
    }

    private void Update()
    {
        OnPreCursorUpdate?.Invoke();

        foreach (GameCursor cursor in cursors.Values)
        {
            cursor.UpdateCursor();
        }
    }


}

[System.Serializable]
public struct GameCursorInputData
{
    public string id;
    public bool isPointerDown;
    public Vector2 screenPos;
    public float rotation;
}

[System.Serializable]
public struct OrientationMessageData
{
    public string header;
    public string id;
    public Vector3 rotation;
    public bool isPointerDown;
}