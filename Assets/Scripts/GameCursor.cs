using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Add this for UI handling
using TMPro;

public class GameCursor : MonoBehaviour
{
    public string id;
    public float smoothness;

    private ElasticScale elasticScale;
    private float pointerDownScaleForce = -50;

    public bool isPointerDown = false;
    private bool lastIsPointerDown = false;
    private bool isDragging = false;

    public Vector2 targetScreenPos;
    public float targetRotation;

    public GameObject lastPointerOverObject;
    public GameObject selectedObject;

    public TextMeshProUGUI idText;

    private GraphicRaycaster uiRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    public float pitchCenterAngle;
    public float yawCenterAngle;



    private void Awake()
    {
        elasticScale = this.GetComponent<ElasticScale>();
        eventSystem = EventSystem.current;
        uiRaycaster = FindObjectOfType<GraphicRaycaster>(); // Assuming there's one in the scene
    }

    public void Initialize(string _id)
    {
        id = _id;
        idText.text = id;
    }

    public void UpdateCursor()
    {
        this.transform.position = Vector2.Lerp(this.transform.position, targetScreenPos, smoothness * Time.deltaTime);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, targetRotation));

        //this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.LerpAngle(this.transform.rotation.eulerAngles.z, -targetRotation, smoothness * Time.deltaTime)));
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = this.transform.position,
            clickCount = isPointerDown ? 1 : 0,
            button = PointerEventData.InputButton.Left
        };

        // Handle UI interactions first
        List<RaycastResult> uiRaycastResults = new List<RaycastResult>();

        uiRaycaster.enabled = true;
        uiRaycaster.Raycast(pointerEventData, uiRaycastResults);
        uiRaycaster.enabled = false;

        if (uiRaycastResults.Count > 0)
        {
            HandleUIEvents(uiRaycastResults[0].gameObject);
        }
        else
        {
            // Handle 3D object interactions
            Ray ray = Camera.main.ScreenPointToRay(pointerEventData.position);
            RaycastHit hit;

            GameObject pointerOverObject = null;

            if (Physics.Raycast(ray, out hit))
            {
                pointerOverObject = hit.collider.gameObject;
            }

            Handle3DObjectEvents(pointerOverObject);
        }

        lastIsPointerDown = isPointerDown;
    }

    private void HandleUIEvents(GameObject pointerOverObject)
    {
        if (lastIsPointerDown != isPointerDown)
        {
            if (isPointerDown)
            {
                OnPointerDown(pointerEventData);
                ExecuteEvents.Execute(pointerOverObject, pointerEventData, ExecuteEvents.pointerDownHandler);
                selectedObject = pointerOverObject;
                isDragging = true;
            }
            else
            {
                OnPointerUp(pointerEventData);
                ExecuteEvents.Execute(selectedObject, pointerEventData, ExecuteEvents.pointerUpHandler);
                selectedObject = null;
                isDragging = false;
            }
        }

        if (selectedObject != null && isPointerDown && isDragging)
        {
            ExecuteEvents.Execute(selectedObject, pointerEventData, ExecuteEvents.dragHandler);
        }

        if (pointerOverObject != lastPointerOverObject)
        {
            if (lastPointerOverObject != null)
            {
                ExecuteEvents.Execute(lastPointerOverObject, pointerEventData, ExecuteEvents.pointerExitHandler);
            }

            ExecuteEvents.Execute(pointerOverObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
            lastPointerOverObject = pointerOverObject;
        }
    }

    private void Handle3DObjectEvents(GameObject pointerOverObject)
    {
        if (lastIsPointerDown != isPointerDown)
        {
            if (isPointerDown)
            {
                OnPointerDown(pointerEventData);

                if (pointerOverObject)
                {
                    ExecuteEvents.Execute(pointerOverObject, pointerEventData, ExecuteEvents.pointerDownHandler);
                    selectedObject = pointerOverObject;
                    isDragging = true;
                }
            }
            else
            {
                OnPointerUp(pointerEventData);

                if (selectedObject)
                {
                    ExecuteEvents.Execute(selectedObject, pointerEventData, ExecuteEvents.pointerUpHandler);
                    selectedObject = null;
                }

                isDragging = false;
            }
        }

        if (selectedObject != null && isPointerDown && isDragging)
        {
            ExecuteEvents.Execute(selectedObject, pointerEventData, ExecuteEvents.dragHandler);
        }

        if (pointerOverObject != null)
        {
            if (pointerOverObject != lastPointerOverObject)
            {
                if (lastPointerOverObject != null)
                {
                    ExecuteEvents.Execute(lastPointerOverObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                }

                ExecuteEvents.Execute(pointerOverObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
                lastPointerOverObject = pointerOverObject;
            }
        }
        else
        {
            if (lastPointerOverObject != null)
            {
                ExecuteEvents.Execute(lastPointerOverObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                lastPointerOverObject = null;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        elasticScale.scaleForce = pointerDownScaleForce;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        elasticScale.scaleForce = 0;
        elasticScale.Pop();
    }
}