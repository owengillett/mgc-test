using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticScale : MonoBehaviour
{
    public float scaleStiffness = 400;
    public float scaleDamping = 15;

    private float prevScale = 1;
    private float scale = 1;
    private float scaleVel;

    public float popScaleVel = 20;

    public float scaleForce = 0;

    public float targetScale = 1;

    private void FixedUpdate()
    {
        prevScale = scale;
        float scaleAccel = scaleForce + (targetScale - scale) * scaleStiffness - scaleVel * scaleDamping;
        scaleVel += scaleAccel * Time.deltaTime;
        scale += scaleVel * Time.deltaTime;
    }

    private void Update()
    {
        float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        this.transform.localScale = Mathf.Lerp(prevScale, scale, alpha) * Vector3.one;
    }

    public void SetScale (float _scale)
    {
        scale = _scale;
        prevScale = scale;
        this.transform.localScale = scale * Vector3.one;
    }

    public void Pop ()
    {
        scaleVel = popScaleVel;
        FixedUpdate();
        Update();
    }
}
