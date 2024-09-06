using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ElasticScale elasticScale;

    public float pointerDownScaleForce = -100;
    public float enterScaleForce = -50;

    private Plane inputPlane;

    private bool isEnter = false;
    private bool isPointerDown = false;

    private Vector3 offset;

    private void Awake()
    {
        elasticScale = this.GetComponentInChildren<ElasticScale>();

        inputPlane = new Plane(Vector3.up, Vector3.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        float distance;

        if (inputPlane.Raycast(ray, out distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            offset = this.transform.position - worldPosition;
        }

        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        elasticScale.Pop();
        isPointerDown = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        float distance;

        if (inputPlane.Raycast(ray, out distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            transform.position = worldPosition + offset;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
    }

    void Update ()
    {
        elasticScale.scaleForce = isPointerDown ? pointerDownScaleForce : isEnter ? enterScaleForce : 0;
    }
}
