using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
	[Serializable]
	public class UIButtonClickedEvent : UnityEvent { }

	[SerializeField]
	public UIButtonClickedEvent m_OnClick = new UIButtonClickedEvent();

	private ElasticScale elasticScale; 

	protected UIButton()
	{ }

	public UIButtonClickedEvent onClick
	{
		get { return m_OnClick; }
		set { m_OnClick = value; }
	} 

	void Awake ()
    {
		elasticScale = this.GetComponent<ElasticScale>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
		elasticScale.targetScale = .8f;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
        m_OnClick?.Invoke();
		elasticScale.targetScale = 1;
		elasticScale.Pop();
	}
}