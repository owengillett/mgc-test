using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BFS.Essential;

public class HomeScreen : Viewport
{
    public UIButton hostButton;
    public UIButton joinButton;

    private void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            GameManager.Instance.StartHost();
        });

        joinButton.onClick.AddListener(() =>
        {
            GameManager.Instance.StartClient();
        });
    }

    protected override void OnEndHide()
    {

    }

    protected override void OnEndShow()
    {

    }

    protected override void OnStartHide()
    {

    }

    protected override void OnStartShow()
    {

    }
}