using System.Threading.Tasks;
using UnityEngine;

namespace BFS.Essential.TweenCore
{
    public class DoDelay : Tween
    {
        private async Task Delay(float duration, System.Action callback)
        {
            m_Run = true;

            float lerp = 0;
            while (lerp < 1)
            {
                if (m_Run == false)
                    return;

                lerp += Time.unscaledDeltaTime / duration;
                lerp = Mathf.Clamp01(lerp);

                await Task.Yield();
            }

            callback?.Invoke();
            Kill(true);
        }

        public Tween Play(float duration, System.Action callback)
        {
            m_Task = Delay(duration, callback);
            return this;
        }
    }
}