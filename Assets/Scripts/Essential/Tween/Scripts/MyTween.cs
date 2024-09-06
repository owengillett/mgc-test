using BFS.Essential.TweenCore;
using System.Collections.Generic;
using UnityEngine;

namespace BFS.Essential
{
    public static class MyTween
    {
        public static Tween DoFloat(float start, float end, float duration, System.Action<float> callback)
        {
            return new DoFloat().Play(start, end, duration, callback);
        }

        public static Tween DoDelay(float duration, System.Action callback)
        {
            return new DoDelay().Play(duration, callback);
        }

        public static Tween DoColor(Color start, Color end, float duration, System.Action<Color> callback)
        {
            return new DoColor().Play(start, end, duration, callback);
        }

        public static Tween DoVector(Vector3 start, Vector3 end, float duration, System.Action<Vector3> callback)
        {
            return new DoVector().Play(start, end, duration, callback);
        }

        public static Tween DoRotation(Quaternion start, Quaternion end, float duration, System.Action<Quaternion> callback)
        {
            return new DoRotation().Play(start, end, duration, callback);
        }

        public static void KillAll()
        {
            while (Tween._Tweens.Count > 0)
                Tween._Tweens[0].Kill(false);
        }
    }
}
