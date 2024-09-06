using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BFS.Essential;

public class GameUIManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnStartHost += OnStartHosting;
        GameManager.Instance.OnStartClient += OnStartJoining;
    }

    public void OnStartHosting ()
    {
        Viewport.GetViewport<HomeScreen>().Hide(.6f);
        Viewport.GetViewport<HostScreen>().Show(.6f);
    }

    public void OnStartJoining()
    {
        Viewport.GetViewport<HomeScreen>().Hide(.6f);
        Viewport.GetViewport<JoinScreen>().Show(.6f);
    }
}