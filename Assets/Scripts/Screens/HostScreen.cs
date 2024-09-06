using UnityEngine;
using TMPro;
using BFS.Essential;

public class HostScreen : Viewport
{
    public TextMeshProUGUI gamePINText;

    protected override void Awake()
    {
        GameManager.Instance.OnStartHost += OnStartHost;
        base.Awake();
    }

    public void OnStartHost()
    {
        GameManager.Instance.host.OnPINCreated += OnGamePINCreated;
    }

    public void OnGamePINCreated ()
    {
        gamePINText.text = GameManager.Instance.host.gamePIN;
    }
}