using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private void Start()
    {
        GameCursorManager.Instance.OnPreCursorUpdate += OnPreCursorUpdate;
    }

    private void OnPreCursorUpdate ()
    {
        GameCursorInputData inputData = new GameCursorInputData()
        {
            id = "mouse",
            isPointerDown = Input.GetMouseButton(0),
            screenPos = Input.mousePosition,
            rotation = 0
        };

        GameCursorManager.Instance.HandleCursorInputData(inputData);
    }
}