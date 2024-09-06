using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BFS.Essential
{
    public class DoRotation : Tween
    {
        private async Task ValueTo(Quaternion start, Quaternion end, float duration, System.Action<Quaternion> callback)
        {
            m_Run = true;

            float lerp = 0;
            while (lerp < 1)
            {
                if (m_Run == false)
                    return;

                lerp += Time.unscaledDeltaTime / duration;
                lerp = Mathf.Clamp01(lerp);

                callback?.Invoke(Quaternion.LerpUnclamped(start, end, m_Interpolator(lerp)));
                await Task.Yield();
            }
            Kill(true);
        }

        public Tween Play(Quaternion start, Quaternion end, float duration, System.Action<Quaternion> callback)
        {
            m_Task = ValueTo(start, end, duration, callback);
            return this;
        }
    }
}