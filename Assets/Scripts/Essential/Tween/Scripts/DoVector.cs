using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BFS.Essential.TweenCore
{
    public class DoVector : Tween
    {
        private async Task ValueTo(Vector3 start, Vector3 end, float duration, System.Action<Vector3> callback)
        {
            m_Run = true;

            float lerp = 0;
            while (lerp < 1)
            {
                if (m_Run == false)
                    return;

                lerp += Time.unscaledDeltaTime / duration;
                lerp = Mathf.Clamp01(lerp);

                callback?.Invoke(Vector3.LerpUnclamped(start, end, m_Interpolator(lerp)));
                await Task.Yield();
            }
            Kill(true);
        }

        public Tween Play(Vector3 start, Vector3 end, float duration, System.Action<Vector3> callback)
        {
            m_Task = ValueTo(start, end, duration, callback);
            return this;
        }
    }
}