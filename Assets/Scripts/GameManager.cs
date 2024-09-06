using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject testCubePrefab;

    [HideInInspector]
    public HostManager host;
    public HostManager hostPrefab;

    public System.Action OnStartHost;

    [HideInInspector]
    public ClientManager client;
    public ClientManager clientPrefab;

    public System.Action OnStartClient;

    public void StartHost ()
    {
        GameObject testCube = Instantiate(testCubePrefab);
        testCube.transform.SetParent(this.transform);

        host = Instantiate(hostPrefab);
        host.transform.SetParent(this.transform);
        host.TryHost();

        OnStartHost?.Invoke();
    }

    public void StopHost ()
    {
        Destroy(host.gameObject);
    }

    public void StartClient()
    {
        client = Instantiate(clientPrefab);
        client.transform.SetParent(this.transform);
        OnStartClient?.Invoke();
    }

    public void StopClient()
    {
        Destroy(client.gameObject);
    }
}