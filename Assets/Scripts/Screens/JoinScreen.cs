using System.Collections;
using UnityEngine;
using TMPro;
using BFS.Essential;

public class JoinScreen : Viewport
{
    public UIButton enterButton;
    public TextMeshProUGUI gamePINText;

    protected override void Awake()
    {
        enterButton.onClick.AddListener(Enter);
        GameManager.Instance.OnStartClient += OnStartClient;
        base.Awake();
    }

    public void OnStartClient()
    {
        GameManager.Instance.client.OnPINReject += OnPINReject;
    }

    public void Enter()
    {
        string gamePIN = gamePINText.text.Trim();
        //TODO: disable enterButton
        GameManager.Instance.client.TryConnect(gamePIN);
    }

    public void OnPINReject ()
    {
        //TODO: PIN reject animation
    }
}