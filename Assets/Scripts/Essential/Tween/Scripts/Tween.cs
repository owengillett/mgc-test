using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;

namespace BFS.Essential
{
    public abstract class Tween
    {
        public static List<Tween> _Tweens = new List<Tween>();

        protected event System.Action m_OnComplete = null;
        //protected AnimationCurve m_Curve = AnimationCurve.Linear(0,0,1,1);
        protected bool m_Run = false;
        protected Task m_Task = null;

        public Func<float, float> m_Interpolator = delegate (float t) { return t; };

        protected bool Run => m_Run;
        public Task Task => m_Task;

        protected Tween()
        {
            _Tweens.Add(this);
        }

        public virtual Tween OnComplete(System.Action callback)
        {
            m_OnComplete = callback;
            return this;
        }

        public virtual Tween SetCurve(AnimationCurve curve)
        {
            return SetInterpolator(delegate (float t) { return curve.Evaluate(t); });
        }

        public virtual Tween SetInterpolator (Func<float, float> interpolator)
        {
            m_Interpolator = interpolator;
            return this;
        }

        public virtual void Kill(bool playOnComplete = false)
        {
            m_Run = false;
            m_Task = null;

            if (playOnComplete)
                m_OnComplete?.Invoke();

            m_OnComplete = null;

            _Tweens.Remove(this);
        }
    }
}