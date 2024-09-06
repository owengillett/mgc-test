using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BFS.Essential;

public class DemoViewportManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DemoViewport demoViewport = Viewport.GetViewport<DemoViewport>();

            if(demoViewport.IsShow)
            {
                demoViewport.Hide(1, () =>
                {
                    Debug.LogWarning("Hide Complete");
                });
            }
            else
            {
                demoViewport.Show(1, () =>
                {
                    Debug.LogWarning("Show Complete");
                });
            }
        }
    }
}
