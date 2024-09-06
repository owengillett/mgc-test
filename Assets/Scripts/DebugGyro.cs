using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DebugGyro : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Start()
    {
        if (AttitudeSensor.current != null)
        {
            InputSystem.EnableDevice(AttitudeSensor.current);
        }
    }

    void Update()
    {
        if (AttitudeSensor.current != null)
        {
            Quaternion rot = AttitudeSensor.current.attitude.ReadValue();
            text.text = rot.eulerAngles.x.ToString();
        }
    }
}