using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BFS.Essential;

public class DemoTween : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;

    Tween scaleTween = null;

    void Start()
    {
        MyTween.DoDelay(5, ScaleBlock);
    }

    void ScaleBlock()
    {
        scaleTween = MyTween.DoVector(transform.localScale, transform.localScale + Vector3.up * 5, 2, (value) =>
        {
            transform.localScale = value;
        }).SetCurve(curve).OnComplete(ScaleEnd);
    }

    void ScaleEnd()
    {
        Debug.LogWarning("Je s'appelle ROB!");
    }

    private void OnDestroy()
    {
        scaleTween.Kill();
    }
}
