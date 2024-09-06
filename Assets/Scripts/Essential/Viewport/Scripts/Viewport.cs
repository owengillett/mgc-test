using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BFS.Essential
{
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class Viewport : MonoBehaviour
	{
		static Dictionary<Type, Viewport> m_Viewports = new Dictionary<Type, Viewport>();

		public EasingFunction.Ease ease;

		public RectTransform main;
		private float mainTransitionScaleDelta = .25f;
		private float transitionFadeRange = .5f;

		public static t GetViewport<t>() where t : Viewport
		{
			return m_Viewports[typeof(t)].GetComponent<t>();
		}

		[SerializeField]
		protected bool m_VisibleAtStart = false;

		bool m_IsShow = true;
		Tween m_TShowHide = null;
		CanvasGroup m_CanvasGroup = null;
		public CanvasGroup CanvasGroup
		{
			get
			{
				if (m_CanvasGroup == null)
					m_CanvasGroup = GetComponent<CanvasGroup>();

				return m_CanvasGroup;
			}
		}

		public bool IsShow => m_IsShow;

		protected virtual void OnStartHide() { }
		protected virtual void OnEndHide() { }
		protected virtual void OnStartShow() { }
		protected virtual void OnEndShow() { }

		void StopFadeTask()
		{
			if (m_TShowHide != null)
			{
				m_TShowHide.Kill();
				m_TShowHide = null;
			}
		}

		void ShowHide(bool on, float duration, Action callback)
		{
			StopFadeTask();

			m_IsShow = on;

			if (on)
				OnStartShow();
			else
				OnStartHide();

			float startValue = on ? 0 : 1;
			float endValue = on ? 1 : 0;

			m_TShowHide = MyTween.DoFloat(startValue, endValue, duration, (f) =>
			{
				CanvasGroup.alpha = Mathf.Clamp01(1 - (1 - f) / transitionFadeRange);
				main.transform.localScale = Vector3.one * (1 + (1 - f) * (on ? -1 : 1) * mainTransitionScaleDelta);
			}).SetInterpolator(delegate (float t) { return EasingFunction.GetEasingFunction(ease)(0, 1, t); }).OnComplete(() =>
			{
				if (on)
					OnEndShow();
				else
					OnEndHide();

				callback?.Invoke();

				gameObject.SetActive(on);
			});
		}

		void OnOffCanvasGroup(bool on)
		{
			CanvasGroup.interactable = on;
			CanvasGroup.blocksRaycasts = on;
		}

		public void Show(float duration, Action callback = null)
		{
			gameObject.SetActive(true);
			OnOffCanvasGroup(true);

			ShowHide(true, duration, callback);
		}

		public void Hide(float duration, Action callback = null)
		{
			OnOffCanvasGroup(false);

			ShowHide(false, duration, callback);
		}

		protected virtual void Awake()
		{
			m_Viewports.Add(GetType(), this);

			if (!m_VisibleAtStart)
				Hide(0.0f);
		}

		protected virtual void OnDestroy()
		{
			StopFadeTask();
			m_Viewports.Remove(GetType());
		}
	}
}