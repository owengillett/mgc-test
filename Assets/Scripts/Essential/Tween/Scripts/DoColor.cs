 using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BFS.Essential.TweenCore
{
    public class DoColor : Tween
    {
        private async Task ValueTo(Color start, Color end, float duration, System.Action<Color> callback)
        {
            m_Run = true;

            float lerp = 0;
            while (lerp < 1)
            {
                if (m_Run == false)
                    return;

                lerp += Time.unscaledDeltaTime / duration;
                lerp = Mathf.Clamp01(lerp);

                callback?.Invoke(Color.LerpUnclamped(start, end, m_Interpolator(lerp)));
                await Task.Yield();
            }

            Kill(true);
        }

        public Tween Play(Color start, Color end, float duration, System.Action<Color> callback)
        {
            m_Task = ValueTo(start, end, duration, callback);
            return this;
        }
    }
}