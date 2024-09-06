using System.Threading.Tasks;
using UnityEngine;

namespace BFS.Essential.TweenCore
{
    public class DoFloat : Tween
    {
        private async Task ValueTo(float start, float end, float duration, System.Action<float> callback)
        {
            m_Run = true;

            float lerp = 0;
            while (lerp < 1)
            {
                if (m_Run == false)
                    return;
 
                lerp += Time.unscaledDeltaTime / duration;
                lerp = Mathf.Clamp01(lerp);

                callback?.Invoke(Mathf.LerpUnclamped(start, end, m_Interpolator(lerp)));
                await Task.Yield();
            }

            Kill(true);
        }

        public Tween Play(float start, float end, float duration, System.Action<float> callback)
        {
            m_Task = ValueTo(start, end, duration, callback);
            return this;
        }
    }
}